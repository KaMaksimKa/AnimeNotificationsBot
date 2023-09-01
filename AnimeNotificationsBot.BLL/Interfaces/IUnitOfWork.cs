using AnimeNotificationsBot.BLL.Interfaces.Repositories;

namespace AnimeNotificationsBot.BLL.Interfaces
{
    public interface IUnitOfWork
    {
        IAnimeCommentRepository AnimeComments { get; }
        IAnimeNotificationRepository AnimeNotifications { get; }
        IAnimeRepository Animes { get; }
        IAnimeStatusRepository AnimeStatuses { get; }
        IAnimeSubscriptionRepository AnimeSubscriptions { get; }
        IAnimeTypeRepository AnimesTypes { get; }
        IDubbingRepository Dubbing { get; }
        IGenreRepository Genres { get; }
        IMpaaRateRepository MpaaRates { get; }
        IStudioRepository Studios { get; }
        IUserRepository Users { get; }
        Task SaveChangesAsync();
    }
}
