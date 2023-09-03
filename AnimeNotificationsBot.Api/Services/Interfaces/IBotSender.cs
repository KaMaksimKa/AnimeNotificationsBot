using AnimeNotificationsBot.Api.Services.CallbackQueryAnswers;
using AnimeNotificationsBot.Api.Services.Messages.Base;
using AnimeNotificationsBot.Common.Enums;
using Telegram.Bot.Types;

namespace AnimeNotificationsBot.Api.Services.Interfaces
{
    public interface IBotSender
    {
        public Task<Message> SendMessageAsync(TextMessage message,long chatId,
            CancellationToken cancellationToken = default, CommandGroupEnum commandGroup = CommandGroupEnum.None);

        public Task<Message> SendMessageAsync(PhotoMessage message, long chatId,
            CancellationToken cancellationToken = default, CommandGroupEnum commandGroup = CommandGroupEnum.None);

        public Task<List<Message>> SendMessageAsync(MediaGroupMessage message, long chatId,
            CancellationToken cancellationToken = default, CommandGroupEnum commandGroup = CommandGroupEnum.None);

        public Task<Message> EditTextMessageAsync(TextMessage message, long chatId,
            int messageId,CancellationToken cancellationToken = default);

        public Task AnswerCallbackQueryAsync(string callbackQueryId, CallbackQueryAnswer? answer = null, CancellationToken cancellationToken = default);

        public Task DeleteReplyMarkup(long chatId, int messageId, CancellationToken cancellationToken = default);

        public Task DeleteMessageAsync(long chatId, int messageId, CancellationToken cancellationToken = default);
    }
}
