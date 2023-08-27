using AnimeNotificationsBot.BLL.Repositories.Base;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;

namespace AnimeNotificationsBot.BLL.Repositories
{
    public class AnimeRepository : Repository<Anime>
    {
        public AnimeRepository(DataContext context) : base(context)
        {
        }
    }
}
