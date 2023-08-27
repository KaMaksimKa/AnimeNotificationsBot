using System.ComponentModel.DataAnnotations;
using AnimeNotificationsBot.DAL.Interfaces;

namespace AnimeNotificationsBot.DAL.Entities
{
    public class User: IRemoveEntity
    {
        public long Id { get; set; }

        /// <summary> Идентификатор пользователя Телеграм </summary>
        public required long TelegramUserId { get; set; }

        [Required]
        public required string FirstName { get; set; }

        public string? LastName { get; set; }

        public string? UserName { get; set; } 

        /// <summary> Идентификатор чата Телеграм </summary>
        public long TelegramChatId { get; set; }

        public bool IsRemoved { get; set; }

        public virtual ICollection<Anime> Animes { get; set; } = new List<Anime>();
    }
}
