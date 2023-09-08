using System.ComponentModel.DataAnnotations;
using AnimeNotificationsBot.Common.Enums;
using AnimeNotificationsBot.Common.Interfaces;

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

        public CommandStateEnum CommandState { get; set; } = CommandStateEnum.None;

        public virtual List<AnimeSubscription> AnimeSubscriptions { get; set; } = new List<AnimeSubscription>();
        public virtual List<BotMessageGroup> BotMessageGroups { get; set; } = new List<BotMessageGroup>();
    }
}
