using AnimeNotificationsBot.BLL.Repositories.Base;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;

namespace AnimeNotificationsBot.BLL.Repositories
{
    public class AnimeCommentRepository: Repository<AnimeComment>
    {
        public AnimeCommentRepository(DataContext context) : base(context)
        {
        }
    }
}
