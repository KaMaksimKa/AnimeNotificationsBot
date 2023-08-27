using AnimeNotificationsBot.BLL.Repositories.Base;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;

namespace AnimeNotificationsBot.BLL.Repositories
{
    internal class StudioRepository: RemoveRepository<Studio>
    {
        public StudioRepository(DataContext context) : base(context)
        {
        }
    }
}
