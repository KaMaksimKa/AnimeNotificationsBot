using AnimeNotificationsBot.DAL.Interfaces;

namespace AnimeNotificationsBot.DAL.Entities
{
    public class AnimeNotification : IEntity
    {
        public long Id { get; set; }
        public required string TitleAnime { get; set; }
        public required int SerialNumber { get; set; }
        public required string Href { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public bool IsNotified { get; set; }

        public long AnimeId { get; set; }
        public virtual Anime Anime { get; set; } = null!;

        public long DubbingId { get; set; }
        public virtual Dubbing Dubbing { get; set; } = null!;
    }
}
