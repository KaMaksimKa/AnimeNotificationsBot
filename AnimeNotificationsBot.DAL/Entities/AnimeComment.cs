using System.ComponentModel.DataAnnotations.Schema;
using AnimeNotificationsBot.Common.Interfaces;

namespace AnimeNotificationsBot.DAL.Entities
{
    public class AnimeComment : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
        public string? Comment { get; set; }
        public string? AuthorName { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public int? Score { get; set; }

        public long? ParentCommentId { get; set; }
        public virtual AnimeComment? ParentComment { get; set; }
        public virtual List<AnimeComment> Children { get; set; } = new List<AnimeComment>();

        public required long AnimeId { get; set; }
        public virtual Anime Anime { get; set; } = null!;

    }
}
