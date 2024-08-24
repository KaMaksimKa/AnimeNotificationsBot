using AnimeNotificationsBot.Common.Interfaces;

namespace AnimeNotificationsBot.DAL.Entities
{
    public class VideoInfo: IEntity
    {
        public long Id { get; set; }
        public string VideoPlayerLink { get; set; } = null!;

        public long? EpisodeId { get; set; }
        public virtual Episode? Episode { get; set; } = null!;

        public long? DubbingId { get; set; }
        public virtual Dubbing? Dubbing { get; set; } = null!;

        public long? VideoProviderId { get; set; }
        public virtual VideoProvider? VideoProvider { get; set; } = null!;

        public List<Video> Videos { get; set; } = new List<Video>();

    }
}
