using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.BLL.Interfaces.Repositories;
using AnimeNotificationsBot.BLL.Repositories;
using AnimeNotificationsBot.DAL;

namespace AnimeNotificationsBot.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        public IAnimeCommentRepository AnimeComments { get; }
        public IAnimeNotificationRepository AnimeNotifications { get; }
        public IAnimeRepository Animes { get; }
        public IAnimeStatusRepository AnimeStatuses { get; }
        public IAnimeSubscriptionRepository AnimeSubscriptions { get; }
        public IAnimeTypeRepository AnimesTypes { get; }
        public IDubbingRepository Dubbing { get; }
        public IGenreRepository Genres { get; }
        public IMpaaRateRepository MpaaRates { get; }
        public IStudioRepository Studios { get; }
        public IUserRepository Users { get; }

        public UnitOfWork(DataContext context)
        {
            _context = context;

            AnimeComments = new AnimeCommentRepository(context);
            AnimeNotifications = new AnimeNotificationRepository(context);
            Animes = new AnimeRepository(context);
            AnimeStatuses = new AnimeStatusRepository(context);
            AnimeSubscriptions = new AnimeSubscriptionRepository(context);
            AnimesTypes = new AnimeTypeRepository(context);
            Dubbing = new DubbingRepository(context);
            Genres = new GenreRepository(context);
            MpaaRates = new MpaaRateRepository(context);
            Studios = new StudioRepository(context);
            Users = new UserRepository(context);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}
