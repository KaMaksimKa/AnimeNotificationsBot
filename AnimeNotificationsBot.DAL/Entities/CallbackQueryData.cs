using AnimeNotificationsBot.Common.Interfaces;

namespace AnimeNotificationsBot.DAL.Entities
{
    public class CallbackQueryData:IEntity
    {
        public long Id { get; set; }
        public required string Data { get; set; }
    }
}
