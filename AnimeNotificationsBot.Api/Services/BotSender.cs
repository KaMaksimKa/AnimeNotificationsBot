using AnimeNotificationsBot.Api.Services.Interfaces;
using AnimeNotificationsBot.Api.Services.Messages;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace AnimeNotificationsBot.Api.Services
{
    public class BotSender:IBotSender
    {
        private readonly ITelegramBotClient _botClient;

        public BotSender(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        public async Task<Message> SendTextMessageAsync(TextMessage message, long chatId, CancellationToken cancellationToken = default)
        {
            return await _botClient.SendTextMessageAsync(chatId,message.Text,replyMarkup:message.ReplyMarkup,
                cancellationToken:cancellationToken);
        }

        public async Task<Message> EditTextMessageAsync(TextMessage message, long chatId, int messageId,
            CancellationToken cancellationToken = default)
        {
            return await _botClient.EditMessageTextAsync(chatId,messageId, message.Text,
                replyMarkup: message.ReplyMarkup as InlineKeyboardMarkup,cancellationToken: cancellationToken);
        }

        public async Task AnswerCallbackQueryAsync(string callbackQueryId, CancellationToken cancellationToken = default)
        {
            await _botClient.AnswerCallbackQueryAsync(callbackQueryId,cancellationToken:cancellationToken);
        }

        public async Task DeleteReplyMarkup(long chatId, int messageId, CancellationToken cancellationToken = default)
        {
            await _botClient.EditMessageReplyMarkupAsync(chatId,messageId,replyMarkup:null,
                cancellationToken:cancellationToken);
        }
    }
}
