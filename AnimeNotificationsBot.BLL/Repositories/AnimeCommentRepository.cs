using AnimeNotificationsBot.BLL.Interfaces.Repositories;
using AnimeNotificationsBot.BLL.Repositories.Base;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;

namespace AnimeNotificationsBot.BLL.Repositories
{
    public class AnimeCommentRepository: Repository<AnimeComment>, IAnimeCommentRepository
    {
        public AnimeCommentRepository(DataContext context) : base(context)
        {
        }
    }
}
