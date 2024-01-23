using AnimeNotificationsBot.Common.Interfaces;

namespace AnimeNotificationsBot.DAL.Entities
{
    public class Feedback: IEntity
    {
        public long Id { get; set; }
        public string Message { get; set; } = null!;

        public long UserId { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
