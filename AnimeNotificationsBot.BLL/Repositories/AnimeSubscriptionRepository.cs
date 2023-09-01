using AnimeNotificationsBot.BLL.Interfaces.Repositories;
using AnimeNotificationsBot.BLL.Repositories.Base;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;

namespace AnimeNotificationsBot.BLL.Repositories
{
    public class AnimeSubscriptionRepository: RemoveRepository<AnimeSubscription>, IAnimeSubscriptionRepository
    {
        public AnimeSubscriptionRepository(DataContext context) : base(context)
        {
        }
    }
}
