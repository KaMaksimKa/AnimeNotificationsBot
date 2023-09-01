using AnimeNotificationsBot.BLL.Interfaces.Repositories;
using AnimeNotificationsBot.BLL.Repositories.Base;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;

namespace AnimeNotificationsBot.BLL.Repositories
{
    public class MpaaRateRepository: RemoveRepository<MpaaRate>, IMpaaRateRepository
    {
        public MpaaRateRepository(DataContext context) : base(context)
        {
        }
    }
}
