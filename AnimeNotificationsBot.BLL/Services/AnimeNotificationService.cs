using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.BLL.Models.Animes;
using AnimeNotificationsBot.BLL.Models.Dubbing;
using AnimeNotificationsBot.BLL.Models.Notifications;
using AnimeNotificationsBot.DAL;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace AnimeNotificationsBot.BLL.Services
{
    public class AnimeNotificationService : IAnimeNotificationService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public AnimeNotificationService(DataContext? context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<AnimeNotificationWithChatsModel>> GetNewNotificationAsync()
        {
            var notifications = await _context.AnimeNotifications
                .Include(x => x.Anime)
                .Include(x => x.Dubbing)
                .Where(x => !x.IsNotified)
                .ToListAsync();

            notifications.ForEach(x => x.IsNotified = true);
            
            var notificationModels = notifications.Select(x => _mapper.Map<AnimeNotificationModel>(x)).ToList();
            var notificationWithChatsModels = new List<AnimeNotificationWithChatsModel>();

            foreach (var notification in notificationModels)
            {
                var chatIds = await _context.AnimeSubscriptions
                    .Where(x => notification.Anime.Id == x.AnimeId && notification.Dubbing.Id == x.DubbingId && !x.IsRemoved)
                    .Select(x => x.User.TelegramChatId)
                    .ToListAsync();

                notificationWithChatsModels.Add(new AnimeNotificationWithChatsModel()
                {
                    AnimeNotificationModel = notification,
                    ChatIds = chatIds
                });
            }

            await _context.SaveChangesAsync();

            return notificationWithChatsModels;
        }
    }
}
