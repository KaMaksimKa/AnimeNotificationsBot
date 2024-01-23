using AnimeNotificationsBot.Common.Interfaces;

namespace AnimeNotificationsBot.DAL.Entities
{
    public class Episode:IEntity,IHasUniqueProperty
    {
        public long Id { get; set; }
        public long EpisodeIdFromAnimeGo { get; set; }
        public int? Number { get; set; }
        public string? Title { get; set; }
        public DateTimeOffset? Released { get; set; }
        public int? Type { get; set; }
        public string? Description { get; set; }

        public long AnimeId { get; set; }
        public virtual Anime Anime { get; set; } = null!;

        public virtual List<VideoInfo> VideoInfos { get; set; } = new List<VideoInfo>();
        public virtual List<AnimeNotification> AnimeNotifications { get; set; } = new List<AnimeNotification> ();

        public object UniqueProperty => EpisodeIdFromAnimeGo;
    }
}
