using AnimeNotificationsBot.Common.Interfaces;

namespace AnimeNotificationsBot.DAL.Entities
{
    public class Dubbing: IEntity, ITitleEntity
    {
        public long Id { get; set; } 
        public required string Title { get; set; }

        public virtual List<AnimeNotification> AnimeNotifications { get; set; } = new List<AnimeNotification>();
        public virtual List<Anime> Animes { get; set; } = new List<Anime>();
        public virtual List<Anime> FirstEpisodeAnimes { get; set; } = new List<Anime>();
        public virtual List<AnimeSubscription> AnimeSubscriptions { get; set; } = new List<AnimeSubscription>();
    }
}
