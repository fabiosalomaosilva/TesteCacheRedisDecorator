using System.ComponentModel.DataAnnotations;
using System.Reflection;
using TesteCacheRedisDecorator.Attributes;
using TesteCacheRedisDecorator.Contexts;
using TesteCacheRedisDecorator.Infraestructure.Interfaces;

namespace TesteCacheRedisDecorator.Infraestructure.Repositories;

public class CachedRepository<T> : IRepository<T> where T : class
{
    private readonly IRepository<T> _innerRepository;
    private readonly RedisContext _redisContext;
    private readonly bool _useCache;
    private readonly HashSet<string> _methodsNotCached;

    public CachedRepository(IRepository<T> innerRepository, RedisContext redisContext)
    {
        _innerRepository = innerRepository;
        _redisContext = redisContext;
        _useCache = Attribute.IsDefined(typeof(T), typeof(CachedAttribute));

        _methodsNotCached = new HashSet<string>();

        foreach (var method in _innerRepository.GetType().GetMethods())
        {
            var attr = method.GetCustomAttribute<CachedAttribute>();
            if (attr != null && !attr.UseCache)
            {
                _methodsNotCached.Add(method.Name);
            }
        }

    }

    private bool ShouldUseCache(string methodName)
    {
        return _useCache && !_methodsNotCached.Contains(methodName);
    }

    public async Task<List<T>> GetAllAsync(int take, int skip)
    {
        var key = $"{typeof(T).Name}:All";

        if (ShouldUseCache(nameof(GetAsync)))
        {
            var cachedValues = await _redisContext.GetAsync<List<T>>(key);
            if (cachedValues is { Count: > 0 })
                return cachedValues;
        }

        var dbValues = await _innerRepository.GetAllAsync(take, skip);

        if (ShouldUseCache(nameof(GetAsync)) && dbValues is { Count: > 0 })
            await _redisContext.SetAsync(key, dbValues);

        return dbValues;
    }

    public async Task<T> GetAsync(int id)
    {
        var key = $"{typeof(T).Name}:{id}";
        T result = default;

        if (ShouldUseCache(nameof(GetAsync)))
        {
            result = await _redisContext.GetAsync<T>(key);
        }

        if (result != null) return result;
        result = await _innerRepository.GetAsync(id);

        if (ShouldUseCache(nameof(GetAsync)) && result != null)
        {
            await _redisContext.SetAsync(key, result);
        }

        return result;
    }

    public async Task<T> AddAsync(T obj)
    {
        var type = typeof(T);
        var keyProperty = type.GetProperties()
                            .FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(KeyAttribute)));

        var result = await _innerRepository.AddAsync(obj);
        if (!ShouldUseCache(nameof(AddAsync))) return result;

        if (keyProperty != null)
        {
            var keyValue = keyProperty.Name;
            var key = $"{type.Name}:{keyValue}";

            await _redisContext.RemoveAsync(key);
        }

        var allKey = $"{typeof(T).Name}:All";
        await _redisContext.RemoveAsync(allKey);
        return result;
    }

    public async Task EditAsync(T obj)
    {
        var type = typeof(T);
        var keyProperty = type.GetProperties()
                            .FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(KeyAttribute)));

        await _innerRepository.EditAsync(obj);

        if (ShouldUseCache(nameof(EditAsync)))
        {
            if (keyProperty != null)
            {
                var keyValue = keyProperty.Name;
                var key = $"{type.Name}:{keyValue}";

                await _redisContext.RemoveAsync(key);
            }

            var allKey = $"{typeof(T).Name}:All";
            await _redisContext.RemoveAsync(allKey);
        }
    }

    public async Task DeleteAsync(int id)
    {
        await _innerRepository.DeleteAsync(id);
        if (ShouldUseCache(nameof(DeleteAsync)))
        {
            var key = $"{typeof(T).Name}:{id}";
            await _redisContext.RemoveAsync(key);

            var allKey = $"{typeof(T).Name}:All";
            await _redisContext.RemoveAsync(allKey);
        }
    }

}