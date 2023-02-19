using Telegram.Bot.Types.ReplyMarkups;

namespace AnimeNotificationsBot.Api.Services.Messages
{
    public class TextMessage
    {
        public string Text { get; set; } = "DefaultMessage";
        public IReplyMarkup? ReplyMarkup { get; set; }
    }
}
