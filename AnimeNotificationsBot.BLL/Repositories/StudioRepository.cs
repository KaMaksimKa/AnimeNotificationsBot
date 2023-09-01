using AnimeNotificationsBot.BLL.Interfaces.Repositories;
using AnimeNotificationsBot.BLL.Repositories.Base;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;

namespace AnimeNotificationsBot.BLL.Repositories
{
    internal class StudioRepository: RemoveRepository<Studio>, IStudioRepository
    {
        public StudioRepository(DataContext context) : base(context)
        {
        }
    }
}
