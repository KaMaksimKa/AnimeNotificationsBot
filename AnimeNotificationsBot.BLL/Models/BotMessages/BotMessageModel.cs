using AnimeNotificationsBot.Common.Enums;

namespace AnimeNotificationsBot.BLL.Models.BotMessages
{
    public class BotMessageModel
    {
        public long ChatId { get; set; }
        public int MessageId { get; set; }
        public CommandGroupEnum CommandGroup { get; set; }
    }
}
