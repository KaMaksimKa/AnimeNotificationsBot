using AnimeNotificationsBot.DAL.Interfaces;

namespace AnimeNotificationsBot.DAL.Entities
{
    public class AnimeComment : IEntity
    {
        public long Id { get; set; }
        public required long CommentId { get; set; }
        public string? Comment { get; set; }
        public string? AuthorName { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public int? Score { get; set; }

        public long? ParentCommentId { get; set; }
        public virtual AnimeComment? ParentComment { get; set; }
        public virtual ICollection<AnimeComment> Children { get; set; } = new List<AnimeComment>();

        public required long AnimeId { get; set; }
        public virtual Anime Anime { get; set; } = null!;

    }
}
