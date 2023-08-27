using AnimeNotificationsBot.BLL.Repositories.Base;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;

namespace AnimeNotificationsBot.BLL.Repositories
{
    public class MpaaRateRepository: RemoveRepository<MpaaRate>
    {
        public MpaaRateRepository(DataContext context) : base(context)
        {
        }
    }
}
