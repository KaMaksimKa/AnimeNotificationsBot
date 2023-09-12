using AnimeNotificationsBot.Api.CallbackQueryAnswers;
using AnimeNotificationsBot.Api.Messages.Base;

namespace AnimeNotificationsBot.Api.Services.Interfaces
{
    public interface IBotSender
    {
        public Task ReplaceMessageAsync(ITelegramMessage newMessage, int oldMessageId, long chatId,
            CancellationToken cancellationToken = default);
        public Task SendMessageAsync(ITelegramMessage message, long chatId,
            CancellationToken cancellationToken = default);

        public Task EditTextMessageAsync(TextMessage message, int messageId, long chatId,
            CancellationToken cancellationToken = default);

        public Task EditReplyMarkupAsync(TextMessage message, int messageId, long chatId,
            CancellationToken cancellationToken = default);

        public Task AnswerCallbackQueryAsync(string callbackQueryId, CallbackQueryAnswer? answer = null,
            CancellationToken cancellationToken = default);

        public Task DeleteReplyMarkup(long chatId, int messageId, CancellationToken cancellationToken = default);

        public Task DeleteMessageAsync(long chatId, int messageId, CancellationToken cancellationToken = default);
    }
}
