using AnimeNotificationsBot.BLL.Repositories.Base;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;

namespace AnimeNotificationsBot.BLL.Repositories
{
    public class UserRepository: RemoveRepository<User>
    {
        public UserRepository(DataContext context) : base(context)
        {
        }
    }
}
