using AnimeNotificationsBot.Api.Services.CallbackQueryAnswers;
using AnimeNotificationsBot.Api.Services.Interfaces;
using AnimeNotificationsBot.Api.Services.Messages.Base;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.BLL.Models.BotMessageGroups;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace AnimeNotificationsBot.Api.Services
{
    public class BotSender : IBotSender
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IBotMessageGroupService _botMessageGroupService;

        public BotSender(ITelegramBotClient botClient, IBotMessageGroupService botMessageGroupService)
        {
            _botClient = botClient;
            _botMessageGroupService = botMessageGroupService;
        }

        public async Task ReplaceMessageAsync(ITelegramMessage newMessage, int oldMessageId, long chatId,
            CancellationToken cancellationToken = default)
        {
            var botMessageGroupModel = await _botMessageGroupService.GetByMessageIdAsync(oldMessageId);

            if (botMessageGroupModel != null)
            {
                var deletedTasks = new List<Task>();

                foreach (var messageId in botMessageGroupModel.MessageIds)
                {
                    deletedTasks.Add(DeleteMessageAsync(botMessageGroupModel.ChatId, messageId, cancellationToken));
                }

                await Task.WhenAll(deletedTasks);
            }

            await SendMessageAsync(newMessage, chatId, cancellationToken);

        }

        public async Task SendMessageAsync(ITelegramMessage message, long chatId, CancellationToken cancellationToken = default)
        {
            var resultMessages = new List<Message>();

            switch (message)
            {
                case PhotoMessage photoMessage:
                    resultMessages.Add(await SendMessageAsync(photoMessage, chatId, cancellationToken));
                    break;
                case TextMessage textMessage:
                    resultMessages.Add(await SendMessageAsync(textMessage, chatId, cancellationToken));
                    break;
                case MediaGroupMessage mediaGroupMessage:
                    resultMessages.AddRange(await SendMessageAsync(mediaGroupMessage, chatId, cancellationToken));
                    break;
                case CombiningMessage combiningMessage:
                    resultMessages.AddRange(await SendMessageAsync(combiningMessage, chatId, cancellationToken));
                    break;
            }

            await _botMessageGroupService.AddAsync(new BotMessageGroupModel()
            {
                ChatId = chatId,
                MessageIds = resultMessages.Select(x => x.MessageId).ToList()
            });
        }

        public async Task EditTextMessageAsync(TextMessage message, int messageId, long chatId,
            CancellationToken cancellationToken = default)
        {
            await _botClient.EditMessageTextAsync(chatId, messageId, message.Text,
                replyMarkup: message.ReplyMarkup as InlineKeyboardMarkup, cancellationToken: cancellationToken);
        }

        public async Task EditReplyMarkupAsync(TextMessage message, int messageId, long chatId,
            CancellationToken cancellationToken = default)
        {
            await _botClient.EditMessageReplyMarkupAsync(chatId, messageId, 
                replyMarkup: message.ReplyMarkup as InlineKeyboardMarkup, cancellationToken: cancellationToken);
        }


        public async Task AnswerCallbackQueryAsync(string callbackQueryId, CallbackQueryAnswer? answer = null, CancellationToken cancellationToken = default)
        {
            await _botClient.AnswerCallbackQueryAsync(callbackQueryId, answer?.Text, answer?.ShowAlert, cancellationToken: cancellationToken);
        }

        public async Task DeleteReplyMarkup(long chatId, int messageId, CancellationToken cancellationToken = default)
        {
            await _botClient.EditMessageReplyMarkupAsync(chatId, messageId, cancellationToken: cancellationToken);
        }

        public async Task DeleteMessageAsync(long chatId, int messageId, CancellationToken cancellationToken = default)
        {
            await _botClient.DeleteMessageAsync(chatId, messageId, cancellationToken);
        }


        private async Task<List<Message>> SendMessageAsync(CombiningMessage message, long chatId,
            CancellationToken cancellationToken = default)
        {
            var resultMessages = new List<Message>();

            foreach (var childMessage in message.Messages)
            {
                switch (childMessage)
                {
                    case PhotoMessage photoMessage:
                        resultMessages.Add(await SendMessageAsync(photoMessage, chatId, cancellationToken));
                        break;
                    case TextMessage textMessage:
                        resultMessages.Add(await SendMessageAsync(textMessage, chatId, cancellationToken));
                        break;
                    case MediaGroupMessage mediaGroupMessage:
                        resultMessages.AddRange(await SendMessageAsync(mediaGroupMessage, chatId, cancellationToken));
                        break;
                }
            }

            return resultMessages;
        }

        private async Task<Message> SendMessageAsync(TextMessage message, long chatId, CancellationToken cancellationToken = default)
        {
            return await _botClient.SendTextMessageAsync(chatId, message.Text, replyMarkup: message.ReplyMarkup, parseMode: message.ParseMode,
                cancellationToken: cancellationToken);
        }


        private async Task<Message> SendMessageAsync(PhotoMessage message, long chatId, CancellationToken cancellationToken = default)
        {

            if ( message.ImgHref == null)
            {
                return await SendMessageAsync((TextMessage)message, chatId, cancellationToken);
            }
            else
            {
                return await _botClient.SendPhotoAsync(chatId, new InputFileUrl(message.ImgHref),caption: message.Text, replyMarkup: message.ReplyMarkup,
                    cancellationToken: cancellationToken);
            }
        }

        private async Task<List<Message>> SendMessageAsync(MediaGroupMessage message, long chatId, CancellationToken cancellationToken = default)
        {
            var media = new List<IAlbumInputMedia>();

            foreach (var image in message.Images)
            {
                if (image.ImgHref != null)
                {
                    media.Add(new InputMediaPhoto(new InputFileUrl(image.ImgHref))
                    {
                        Caption = image.Caption,
                    });
                }
            }

            return (await _botClient.SendMediaGroupAsync(chatId, media, cancellationToken: cancellationToken)).ToList();
        }

    }
}
