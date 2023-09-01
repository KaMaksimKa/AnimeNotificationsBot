using AnimeNotificationsBot.BLL.Interfaces.Repositories;
using AnimeNotificationsBot.BLL.Repositories.Base;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;

namespace AnimeNotificationsBot.BLL.Repositories
{
    public class DubbingRepository: RemoveRepository<Dubbing>, IDubbingRepository
    {
        public DubbingRepository(DataContext context) : base(context)
        {
        }
    }
}
