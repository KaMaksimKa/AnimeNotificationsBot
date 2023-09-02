using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    }
}
