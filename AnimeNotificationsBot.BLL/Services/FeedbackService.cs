using AnimeNotificationsBot.BLL.Helpers;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;

namespace AnimeNotificationsBot.BLL.Services
{
    public class FeedbackService: IFeedbackService
    {
        private readonly DataContext _context;

        public FeedbackService(DataContext context)
        {
            _context = context;
        }

        public async Task AddAsync(string message, long telegramUserId)
        {
            var user = await _context.Users.GetUserByTelegramId(telegramUserId);

            await _context.Feedbacks.AddAsync(new Feedback()
            {
                User = user,
                Message = message,
            });

            await _context.SaveChangesAsync();
        }
    }
}
