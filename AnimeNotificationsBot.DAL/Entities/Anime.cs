using AnimeNotificationsBot.Common.Interfaces;

namespace AnimeNotificationsBot.DAL.Entities
{
    public class Anime: IEntity
    {
        public long Id { get; set; }
        public required long AnimeIdFromAnimeGo { get; set; }
        public string? TitleEn { get; set; }
        public string? TitleRu { get; set; }
        public int? Year { get; set; }
        public string? Description { get; set; }
        public double? Rate { get; set; }
        public int? CountEpisode { get; set; }
        public int? Planned { get; set; }
        public int? Completed { get; set; }
        public int? Watching { get; set; }
        public int? Dropped { get; set; }
        public int? OnHold { get; set; }
        public string? Href { get; set; }
        public string? ImgIdFromAnimeGo { get; set; }
        public string? ImgHref { get; set; }
        public string? NextEpisode { get; set; }
        public string? Duration { get; set; }
        public long? IdForComments { get; set; }

        public long? StatusId { get; set; }
        public virtual AnimeStatus? Status { get; set; }

        public long? MpaaRateId { get; set; }
        public virtual MpaaRate? MpaaRate { get; set; }

        public long? TypeId { get; set; }
        public virtual AnimeType? Type { get; set; }

        public virtual List<Studio> Studios { get; set; } = new List<Studio>();
        public virtual List<Genre> Genres { get; set; } = new List<Genre>();
        public virtual List<Dubbing> Dubbing { get; set; } = new List<Dubbing>();
        public virtual List<AnimeComment> Comments { get; set; } = new List<AnimeComment>();
        public virtual List<AnimeSubscription> AnimeSubscriptions { get; set; } = new List<AnimeSubscription>();
        public virtual List<AnimeNotification> AnimeNotifications { get; set; } = new List<AnimeNotification>();
        public virtual List<Episode> Episodes { get; set; } = new List<Episode>();
        

    }
}
