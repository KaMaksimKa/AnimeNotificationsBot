using AnimeNotificationsBot.BLL.Interfaces.Repositories;
using AnimeNotificationsBot.BLL.Repositories.Base;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;

namespace AnimeNotificationsBot.BLL.Repositories
{
    public class AnimeTypeRepository: RemoveRepository<AnimeType>, IAnimeTypeRepository
    {
        public AnimeTypeRepository(DataContext context) : base(context)
        {
        }
    }
}
