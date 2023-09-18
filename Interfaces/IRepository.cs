using BlogAPI.Interfaces;

namespace BlogAPI.Interfaces
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<List<T>> GetAllAsync(int id);
        Task<T?> GetAsync(int id);
        Task<T?> AddAsync(T entity);
        Task<T?> UpdateAsync(T entity);
        Task<T?> DeleteAsync(int id);
    }
}
