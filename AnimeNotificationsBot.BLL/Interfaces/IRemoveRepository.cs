using AnimeNotificationsBot.DAL.Interfaces;

namespace AnimeNotificationsBot.BLL.Interfaces
{
    public interface IRemoveRepository<T>: IRepository<T> where T :class, IRemoveEntity
    {
        Task<IEnumerable<T>> GetRangeWhereAsync(Func<IQueryable<T>, Task<IEnumerable<T>>> selector, bool includingDeleted = false);
        Task<T> GetWhereAsync(Func<IQueryable<T>, Task<T>> selector, bool includingDeleted = false);
        Task<T> GetById(int id, bool includingDeleted = false);
        Task<List<T>> GetAll(bool includingDeleted = false);
        Task RemoveById(int id, bool permanently = false);
    }
}
