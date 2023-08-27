using AnimeNotificationsBot.DAL.Interfaces;

namespace AnimeNotificationsBot.DAL.Entities
{
    public class Dubbing: IRemoveEntity
    {
        public long Id { get; set; } 
        public required string Title { get; set; }
        public bool IsRemoved { get; set; }

        public virtual ICollection<AnimeNotification> AnimeNotifications { get; set; } = new List<AnimeNotification>();
        public virtual ICollection<Anime> Animes { get; set; } = new List<Anime>();
        public virtual ICollection<Anime> FirstEpisodeAnimes { get; set; } = new List<Anime>();
    }
}
