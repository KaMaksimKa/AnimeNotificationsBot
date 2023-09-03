namespace AnimeNotificationsBot.BLL.Interfaces
{
    public interface ICallbackQueryDataService
    {
        public Task<long> AddAsync<T>(T data);
        public Task<T> GetAsync<T>(long dataId);
    }
}
