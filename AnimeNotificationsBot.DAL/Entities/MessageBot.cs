using AnimeNotificationsBot.Common.Enums;
using AnimeNotificationsBot.Common.Interfaces;

namespace AnimeNotificationsBot.DAL.Entities
{
    public class BotMessage:IRemoveEntity
    {
        public long Id { get; set; }
        public int MessageId { get; set; }
        public CommandGroupEnum CommandGroup { get; set; }
        public bool IsRemoved { get; set; }

        public long UserId { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
