using AnimeNotificationsBot.Common.Interfaces;

namespace AnimeNotificationsBot.BLL.Interfaces.Repositories
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<List<T>> GetRangeWhereAsync(Func<IQueryable<T>, Task<List<T>>> selector);
        Task<T?> GetFirstOrDefaultAsync(Func<IQueryable<T>, Task<T?>> selector);
        Task<T> GetById(int id);
        Task<List<T>> GetAll();
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task RemoveById(int id);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void Remove(T id);
        void RemoveRange(IEnumerable<T> entities);

    }
}
