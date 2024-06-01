using StackExchange.Redis;
using System.Text.Json;

namespace TesteCacheRedisDecorator.Contexts;

public class RedisContext
{
    private readonly IConnectionMultiplexer _redisConnection;

    public RedisContext(IConfiguration configuration)
    {
        var connectionString = configuration.GetSection("Redis:Configuration").Value;
        _redisConnection = ConnectionMultiplexer.Connect(connectionString);
    }


    private IDatabase GetDatabase() => _redisConnection.GetDatabase();

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var serializedValue = JsonSerializer.Serialize(value);
        await GetDatabase().StringSetAsync(key, serializedValue, expiry);
    }

    public async Task<T> GetAsync<T>(string key)
    {
        var serializedValue = await GetDatabase().StringGetAsync(key);
        return serializedValue.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>(serializedValue);
    }

    public async Task RemoveAsync(string key)
    {
        await GetDatabase().KeyDeleteAsync(key);
    }
}