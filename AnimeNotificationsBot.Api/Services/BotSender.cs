using AnimeNotificationsBot.Api.Services.CallbackQueryAnswers;
using AnimeNotificationsBot.Api.Services.Interfaces;
using AnimeNotificationsBot.Api.Services.Messages.Base;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
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

        public async Task<Message> SendMessageAsync(TextMessage message, long chatId, CancellationToken cancellationToken = default)
        {
            return await _botClient.SendTextMessageAsync(chatId,message.Text,replyMarkup:message.ReplyMarkup, parseMode:message.ParseMode,
                cancellationToken:cancellationToken);
        }

        public async Task<Message> SendMessageAsync(PhotoMessage message, long chatId, CancellationToken cancellationToken = default)
        {
            return await _botClient.SendPhotoAsync(chatId,new InputOnlineFile(message.Content,message.FileName), message.Text, replyMarkup: message.ReplyMarkup,
                cancellationToken: cancellationToken);
        }

        public async Task<Message> EditTextMessageAsync(TextMessage message, long chatId, int messageId,
            CancellationToken cancellationToken = default)
        {
            return await _botClient.EditMessageTextAsync(chatId,messageId, message.Text,
                replyMarkup: message.ReplyMarkup as InlineKeyboardMarkup,cancellationToken: cancellationToken);
        }

        public async Task AnswerCallbackQueryAsync(string callbackQueryId, CallbackQueryAnswer answer, CancellationToken cancellationToken = default)
        {
            await _botClient.AnswerCallbackQueryAsync(callbackQueryId, answer.Text, answer.ShowAlert, cancellationToken: cancellationToken);
        }

        public async Task DeleteReplyMarkup(long chatId, int messageId, CancellationToken cancellationToken = default)
        {
            await _botClient.EditMessageReplyMarkupAsync(chatId, messageId, cancellationToken: cancellationToken);
        }

        public async Task DeleteMessageAsync(long chatId, int messageId, CancellationToken cancellationToken = default)
        {
            await _botClient.DeleteMessageAsync(chatId, messageId, cancellationToken);
        }
    }
}
