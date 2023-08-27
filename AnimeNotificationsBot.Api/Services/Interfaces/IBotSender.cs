using AnimeNotificationsBot.Api.Services.CallbackQueryAnswers;
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

        public Task AnswerCallbackQueryAsync(string callbackQueryId, CallbackQueryAnswer answer, CancellationToken cancellationToken = default);

        public Task DeleteReplyMarkup(long chatId, int messageId, CancellationToken cancellationToken = default);
    }
}
