
using AnimeNotificationsBot.DAL.Interfaces;

namespace AnimeNotificationsBot.DAL.Entities
{
    public class Genre: IRemoveEntity
    {
        public long Id { get; set; }
        public required string Title { get; set; }

        public bool IsRemoved { get; set; }

        public virtual ICollection<Anime> Animes { get; set; } = new List<Anime>();
    
    }
}
