using AnimeNotificationsBot.BLL.Interfaces.Repositories;
using AnimeNotificationsBot.BLL.Repositories.Base;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;

namespace AnimeNotificationsBot.BLL.Repositories
{
    public class UserRepository: RemoveRepository<User>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context)
        {
        }
    }
}
