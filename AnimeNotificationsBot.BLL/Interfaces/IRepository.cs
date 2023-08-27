using AnimeNotificationsBot.DAL.Interfaces;

namespace AnimeNotificationsBot.BLL.Interfaces
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<IEnumerable<T>> GetRangeWhereAsync(Func<IQueryable<T>, Task<IEnumerable<T>>> selector);
        Task<T> GetWhereAsync(Func<IQueryable<T>, Task<T>> selector);
        Task<T> GetById(int id);
        Task<List<T>> GetAll();
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        Task RemoveById(int id);

    }
}
