using AnimeNotificationsBot.Common.Interfaces;

namespace AnimeNotificationsBot.DAL.Entities
{
    public class Dubbing: IEntity, IHasUniqueProperty
    {
        public long Id { get; set; } 
        public required string Title { get; set; }

        public virtual List<AnimeNotification> AnimeNotifications { get; set; } = new List<AnimeNotification>();
        public virtual List<Anime> Animes { get; set; } = new List<Anime>();
        public virtual List<AnimeSubscription> AnimeSubscriptions { get; set; } = new List<AnimeSubscription>();
        public virtual List<VideoInfo> VideoInfos { get; set; } = new List<VideoInfo>();
        public object UniqueProperty => Title;
    }
}
