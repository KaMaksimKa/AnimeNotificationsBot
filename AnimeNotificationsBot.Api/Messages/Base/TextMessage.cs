using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace AnimeNotificationsBot.Api.Messages.Base
{
    public class TextMessage : ITelegramMessage
    {
        public string Text { get; set; } = "DefaultMessage";
        public IReplyMarkup? ReplyMarkup { get; set; }
        public ParseMode? ParseMode { get; set; }
    }
}
