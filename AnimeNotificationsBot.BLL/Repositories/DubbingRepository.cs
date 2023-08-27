using AnimeNotificationsBot.BLL.Repositories.Base;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;

namespace AnimeNotificationsBot.BLL.Repositories
{
    public class DubbingRepository: RemoveRepository<Dubbing>
    {
        public DubbingRepository(DataContext context) : base(context)
        {
        }
    }
}
