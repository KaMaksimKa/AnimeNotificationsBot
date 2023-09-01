using AnimeNotificationsBot.Common.Interfaces;

namespace AnimeNotificationsBot.BLL.Interfaces.Repositories
{
    public interface IRemoveRepository<T> : IRepository<T> where T : class, IRemoveEntity
    {
        Task<List<T>> GetRangeWhereAsync(Func<IQueryable<T>, Task<List<T>>> selector, bool includingDeleted = false);
        Task<T?> GetFirstOrDefaultAsync(Func<IQueryable<T>, Task<T?>> selector, bool includingDeleted = false);
        Task<T> GetById(int id, bool includingDeleted = false);
        Task<List<T>> GetAll(bool includingDeleted = false);
        Task RemoveById(int id, bool permanently = false);
    }
}
