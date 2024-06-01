namespace TesteCacheRedisDecorator.Infraestructure.Interfaces
{
    public interface IRepository<T>
    {
        Task<List<T>> GetAllAsync(int take, int skip);
        Task<T?> GetAsync(int id);
        Task<T> AddAsync(T item);
        Task EditAsync(T item);
        Task DeleteAsync(int id);
    }
}
