using AnimeNotificationsBot.BLL.Models;
using AnimeNotificationsBot.BLL.Models.Subscriptions;

namespace AnimeNotificationsBot.BLL.Interfaces
{
    public interface IAnimeSubscriptionService
    {
        Task<SubscribedAnimeModel> SubscribeAsync(SubscribeAnimeModel model, long telegramUserId);
        Task<SubscribedAnimeModel> UnsubscribeAsync(SubscribeAnimeModel model, long telegramUserId);
        Task<List<SubscriptionDubbingModel>> GetUserSubscriptionsByAnimeAsync(long animeId, long telegramUserId);
        Task<UserSubscriptionListModel> GetUserSubscriptionsListModelAsync(PaginationModel pagination,long telegramUserId);
    }
}
