using AnimeNotificationsBot.Common.Interfaces;

namespace AnimeNotificationsBot.DAL.Entities
{
    public class Image:IEntity
    {
        public long Id { get; set; }
        public required string FileName { get; set; }
        public required string Path { get; set; }

        public long AnimeId { get; set; }
        public virtual Anime Anime { get; set; } = null!;
    }
}
