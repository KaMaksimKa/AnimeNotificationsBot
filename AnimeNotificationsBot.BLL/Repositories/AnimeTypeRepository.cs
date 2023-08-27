using AnimeNotificationsBot.BLL.Repositories.Base;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;

namespace AnimeNotificationsBot.BLL.Repositories
{
    public class AnimeTypeRepository: RemoveRepository<AnimeType>
    {
        public AnimeTypeRepository(DataContext context) : base(context)
        {
        }
    }
}
