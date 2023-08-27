using AnimeNotificationsBot.BLL.Repositories.Base;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;

namespace AnimeNotificationsBot.BLL.Repositories
{
    public class AnimeNotificationRepository: Repository<AnimeNotification>
    {
        public AnimeNotificationRepository(DataContext context) : base(context)
        {
        }
    }
}
