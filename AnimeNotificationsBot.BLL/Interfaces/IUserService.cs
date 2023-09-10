using AnimeNotificationsBot.BLL.Models.Users;
using AnimeNotificationsBot.Common.Enums;

namespace AnimeNotificationsBot.BLL.Interfaces
{
    public interface IUserService
    {
        public Task CreateUserAsync(CreateUserModel model);

        public Task DeleteUserAsync(long telegramUserId);

        public Task<CommandStateEnum> GetCommandStateAsync(long telegramUserId);

        public Task SetCommandStateAsync(long telegramUserId, CommandStateEnum commandState);
    }
}
