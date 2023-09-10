using AnimeNotificationsBot.BLL.Models.Notifications;

namespace AnimeNotificationsBot.BLL.Interfaces
{
    public interface IAnimeNotificationService
    {
        public Task<List<AnimeNotificationWithChatsModel>> GetNewNotificationAsync();
    }
}
