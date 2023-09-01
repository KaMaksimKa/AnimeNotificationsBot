using AnimeNotificationsBot.BLL.Interfaces.Repositories;
using AnimeNotificationsBot.BLL.Repositories.Base;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;

namespace AnimeNotificationsBot.BLL.Repositories
{
    public class AnimeStatusRepository: RemoveRepository<AnimeStatus>, IAnimeStatusRepository
    {
        public AnimeStatusRepository(DataContext context) : base(context)
        {
        }
    }
}
