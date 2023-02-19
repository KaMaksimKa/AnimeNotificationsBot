using AnimeNotificationsBot.Api.Services.Messages;
using Telegram.Bot.Types;

namespace AnimeNotificationsBot.Api.Services.Interfaces
{
    public interface IBotSender
    {
        public Task<Message> SendTextMessageAsync(TextMessage message,long chatId,
            CancellationToken cancellationToken = default);

        public Task<Message> EditTextMessageAsync(TextMessage message, long chatId,
            int messageId,CancellationToken cancellationToken = default);

        public Task AnswerCallbackQueryAsync(string callbackQueryId, CancellationToken cancellationToken = default);

        public Task DeleteReplyMarkup(long chatId,
            int messageId, CancellationToken cancellationToken = default);
    }
}
