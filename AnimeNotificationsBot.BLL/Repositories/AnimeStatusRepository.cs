using AnimeNotificationsBot.BLL.Repositories.Base;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;

namespace AnimeNotificationsBot.BLL.Repositories
{
    public class AnimeStatusRepository: RemoveRepository<AnimeStatus>
    {
        public AnimeStatusRepository(DataContext context) : base(context)
        {
        }
    }
}
