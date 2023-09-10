using AnimeNotificationsBot.Common.Interfaces;

namespace AnimeNotificationsBot.DAL.Entities
{
    public class AnimeSubscription: IRemoveEntity
    {
        public long Id { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public bool IsRemoved { get; set; }

        public long UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public long AnimeId { get; set; }
        public virtual Anime Anime { get; set; } = null!;

        public long DubbingId { get; set; }
        public virtual Dubbing Dubbing { get;set; } = null!;

    }
}
