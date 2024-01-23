
using AnimeNotificationsBot.Common.Interfaces;

namespace AnimeNotificationsBot.DAL.Entities
{
    public class Genre: IEntity, IHasUniqueProperty
    {
        public long Id { get; set; }
        public required string Title { get; set; }

        public virtual List<Anime> Animes { get; set; } = new List<Anime>();

        public object UniqueProperty => Title;
    }
}
