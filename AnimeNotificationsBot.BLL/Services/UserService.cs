using AnimeNotificationsBot.BLL.Helpers;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.BLL.NewFolder.NewFolder;
using AnimeNotificationsBot.Common.Enums;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;

namespace AnimeNotificationsBot.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        public async Task CreateUserAsync(CreateUserModel model)
        {
            var user = _context.Users.FirstOrDefault(x => x.TelegramUserId == model.TelegramUserId);

            if (user == null)
            {
                await _context.Users.AddAsync(new User()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    TelegramUserId = model.TelegramUserId,
                    TelegramChatId = model.TelegramChatId,
                    UserName = model.UserName,
                    IsRemoved = false
                });
            }
            else
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.TelegramUserId = model.TelegramUserId;
                user.TelegramChatId = model.TelegramChatId;
                user.UserName = model.UserName;
                user.IsRemoved = false;
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(long telegramUserId)
        {
            var user = await _context.Users.GetUserByTelegramId(telegramUserId);
            user.IsRemoved = true;
            await _context.SaveChangesAsync();
        }

        public async Task<CommandStateEnum> GetCommandStateAsync(long telegramUserId)
        {
            return (await _context.Users.GetUserByTelegramId(telegramUserId)).CommandState;
        }

        public async Task SetCommandStateAsync(long telegramUserId, CommandStateEnum commandState)
        {
            var user = await _context.Users.GetUserByTelegramId(telegramUserId);
            user.CommandState = commandState;
            await _context.SaveChangesAsync();
        }
    }
}
