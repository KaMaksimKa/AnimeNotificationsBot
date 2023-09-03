using AnimeNotificationsBot.Api.Services.CallbackQueryAnswers;
using AnimeNotificationsBot.Api.Services.Messages.Base;
using Telegram.Bot.Types;

namespace AnimeNotificationsBot.Api.Services.Interfaces
{
    public interface IBotSender
    {
        public Task ReplaceMessageAsync(ITelegramMessage newMessage, int oldMessageId, long chatId,
            CancellationToken cancellationToken = default);
        public Task SendMessageAsync(ITelegramMessage message, long chatId,
            CancellationToken cancellationToken = default);

        public Task EditTextMessageAsync(TextMessage message, long chatId,
            int messageId, CancellationToken cancellationToken = default);

        public Task AnswerCallbackQueryAsync(string callbackQueryId, CallbackQueryAnswer? answer = null,
            CancellationToken cancellationToken = default);

        public Task DeleteReplyMarkup(long chatId, int messageId, CancellationToken cancellationToken = default);

        public Task DeleteMessageAsync(long chatId, int messageId, CancellationToken cancellationToken = default);
    }
}
