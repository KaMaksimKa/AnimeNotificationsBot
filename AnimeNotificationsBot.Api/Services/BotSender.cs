using AnimeNotificationsBot.Api.Services.CallbackQueryAnswers;
using AnimeNotificationsBot.Api.Services.Interfaces;
using AnimeNotificationsBot.Api.Services.Messages.Base;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.BLL.Models.BotMessageGroup;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
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

            if (message.Photo == null)
            {
                return await SendMessageAsync((TextMessage)message, chatId, cancellationToken);
            }
            else
            {
                var resultMessage = await _botClient.SendPhotoAsync(chatId, new InputOnlineFile(message.Photo.Content, message.Photo.FileName), message.Text, replyMarkup: message.ReplyMarkup,
                    cancellationToken: cancellationToken);

                message.Photo.Content.Close();

                return resultMessage;
            }
        }

        private async Task<List<Message>> SendMessageAsync(MediaGroupMessage message, long chatId, CancellationToken cancellationToken = default)
        {
            var media = new List<IAlbumInputMedia>();

            foreach (var image in message.Images)
            {
                media.Add(new InputMediaPhoto(new InputMedia(image.Image.Content, image.Image.FileName))
                {
                    Caption = image.Caption,
                });
            }

            var resultMessages = await _botClient.SendMediaGroupAsync(chatId, media, cancellationToken: cancellationToken);

            foreach (var image in message.Images)
            {
                image.Image.Content.Close();
            }

            return resultMessages.ToList();
        }

        public async Task EditTextMessageAsync(TextMessage message, long chatId, int messageId,
            CancellationToken cancellationToken = default)
        {
            await _botClient.EditMessageTextAsync(chatId, messageId, message.Text,
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


    }
}
