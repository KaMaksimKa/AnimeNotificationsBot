using AnimeNotificationsBot.Common.Interfaces;

namespace AnimeNotificationsBot.DAL.Entities
{
    public class BotMessageGroup: IEntity
    {
        public long Id { get; set; }

        public long UserId { get; set; }
        public User User { get; set; } = null!;

        public virtual List<BotMessage> Messages { get; set; } = new List<BotMessage>();
    }
}
