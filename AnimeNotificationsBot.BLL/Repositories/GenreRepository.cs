using AnimeNotificationsBot.BLL.Repositories.Base;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;

namespace AnimeNotificationsBot.BLL.Repositories
{
    public class GenreRepository: RemoveRepository<Genre>
    {
        public GenreRepository(DataContext context) : base(context)
        {
        }
    }
}
