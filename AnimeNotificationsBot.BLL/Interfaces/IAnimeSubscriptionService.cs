using AnimeNotificationsBot.BLL.Models.Subscriptions;

namespace AnimeNotificationsBot.BLL.Interfaces
{
    public interface IAnimeSubscriptionService
    {
        Task<SubscribedAnimeModel> SubscribeAsync(SubscribeAnimeModel model, long telegramUserId);
        Task<SubscribedAnimeModel> UnsubscribeAsync(SubscribeAnimeModel model, long telegramUserId);
        Task<List<SubscriptionDubbingModel>> GetSubscriptionsAsync(long animeId, long telegramUserId);
    }
}
