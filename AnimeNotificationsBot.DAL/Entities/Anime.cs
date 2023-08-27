using AnimeNotificationsBot.DAL.Interfaces;

namespace AnimeNotificationsBot.DAL.Entities
{
    public class Anime: IEntity
    {
        public long Id { get; set; }
        public required long IdFromAnimeGo { get; set; }
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
        public string? ImgHref { get; set; }
        public string? NextEpisode { get; set; }
        public string? Duration { get; set; }
        public long? IdForComments { get; set; }
        public AnimeStatus? Status { get; set; }
        public MpaaRate? MpaaRate { get; set; }
        public AnimeType? Type { get; set; }

        public virtual ICollection<Studio> Studios { get; set; } = new List<Studio>();
        public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();
        public virtual ICollection<Dubbing> Dubbing { get; set; } = new List<Dubbing>();
        public virtual ICollection<Dubbing> DubbingFromFirstEpisode { get; set; } = new List<Dubbing>();
        public virtual ICollection<AnimeComment> Comments { get; set; } = new List<AnimeComment>();
        public virtual ICollection<User> Users { get; set; } = new List<User>();

    }
}
