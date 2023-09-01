using AnimeNotificationsBot.BLL.Interfaces.Repositories;
using AnimeNotificationsBot.BLL.Repositories.Base;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;

namespace AnimeNotificationsBot.BLL.Repositories
{
    public class AnimeRepository : Repository<Anime>, IAnimeRepository
    {
        public AnimeRepository(DataContext context) : base(context)
        {
        }
    }
}
