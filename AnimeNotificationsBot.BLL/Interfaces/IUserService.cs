using AnimeNotificationsBot.BLL.NewFolder.NewFolder;

namespace AnimeNotificationsBot.BLL.Interfaces
{
    public interface IUserService
    {
        public Task CreateUserAsync(CreateUserModel model);

        public Task DeleteUserAsync(long telegramUserId);
    }
}
