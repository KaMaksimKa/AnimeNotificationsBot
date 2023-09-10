using AnimeNotificationsBot.Common.Interfaces;

namespace AnimeNotificationsBot.DAL.Entities
{
    public class AnimeType: IEntity, ITitleEntity
    {
        public long Id { get; set; }
        public required string Title { get; set; }

        public virtual List<Anime> Animes { get; set; } = null!;
        
    }
}
