using AnimeNotificationsBot.Common.Exceptions;
using AnimeNotificationsBot.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace AnimeNotificationsBot.BLL.Helpers
{
    public static class DataContextExtensions
    {
        public static async Task<User> GetUserByTelegramId(this DbSet<User> users, long telegramUserId)
        {
            var user = await users.FirstOrDefaultAsync(x => x.TelegramUserId == telegramUserId);

            if (user == null)
                throw new NotFoundEntityException()
                {
                    EntityName = nameof(User),
                    PropertyName = nameof(user.TelegramUserId),
                    PropertyValue = telegramUserId
                };

            return user;
        }

        public static async Task<User> GetUserByChatId(this DbSet<User> users, long chatId)
        {
            var user = await users.FirstOrDefaultAsync(x => x.TelegramChatId == chatId);

            if (user == null)
                throw new NotFoundEntityException()
                {
                    EntityName = nameof(User),
                    PropertyName = nameof(user.TelegramChatId),
                    PropertyValue = chatId
                };

            return user;
        }
    }
}
