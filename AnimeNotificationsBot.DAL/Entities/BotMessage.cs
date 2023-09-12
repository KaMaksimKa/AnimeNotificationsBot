using AnimeNotificationsBot.Common.Interfaces;

namespace AnimeNotificationsBot.DAL.Entities
{
    public class BotMessage:IEntity
    {
        public long Id { get; set; }
        public int MessageId { get; set; }

        public long BotMessageGroupId { get; set; }
        public virtual BotMessageGroup BotMessageGroup { get; set; } = null!;

    }
}
