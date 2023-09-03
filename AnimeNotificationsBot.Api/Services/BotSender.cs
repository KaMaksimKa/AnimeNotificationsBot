using AnimeNotificationsBot.Api.Services.CallbackQueryAnswers;
using AnimeNotificationsBot.Api.Services.Interfaces;
using AnimeNotificationsBot.Api.Services.Messages.Base;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.BLL.Models.BotMessage;
using AnimeNotificationsBot.Common.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace AnimeNotificationsBot.Api.Services
{
    public class BotSender : IBotSender
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IBotMessageService _botMessageService;

        public BotSender(ITelegramBotClient botClient,IBotMessageService BotMessageService)
        {
            _botClient = botClient;
            _botMessageService = BotMessageService;
        }

        public async Task<Message> SendMessageAsync(TextMessage message, long chatId, CancellationToken cancellationToken = default,
            CommandGroupEnum commandGroup = CommandGroupEnum.None)
        {
            await DeleteBotMessageByCommandGroupAsync(chatId, commandGroup, cancellationToken);

            var resultMessage = await _botClient.SendTextMessageAsync(chatId, message.Text, replyMarkup: message.ReplyMarkup, parseMode: message.ParseMode,
                cancellationToken: cancellationToken);

            await _botMessageService.AddAsync(new BotMessageModel()
            {
                MessageId = resultMessage.MessageId,
                CommandGroup = commandGroup,
                ChatId = chatId,
            });

            return resultMessage;
        }

        public async Task<Message> SendMessageAsync(PhotoMessage message, long chatId, CancellationToken cancellationToken = default,
            CommandGroupEnum commandGroup = CommandGroupEnum.None)
        {
            await DeleteBotMessageByCommandGroupAsync(chatId, commandGroup, cancellationToken);

            if (message.Photo == null)
            {
                return await SendMessageAsync((TextMessage)message, chatId, cancellationToken);
            }
            else
            {
                var resultMessage = await _botClient.SendPhotoAsync(chatId, new InputOnlineFile(message.Photo.Content, message.Photo.FileName), message.Text, replyMarkup: message.ReplyMarkup,
                    cancellationToken: cancellationToken);

                message.Photo.Content.Close();

                await _botMessageService.AddAsync(new BotMessageModel()
                {
                    MessageId = resultMessage.MessageId,
                    CommandGroup = commandGroup,
                    ChatId = chatId,
                });

                return resultMessage;
            }
        }

        public async Task<List<Message>> SendMessageAsync(MediaGroupMessage message, long chatId, CancellationToken cancellationToken = default,
            CommandGroupEnum commandGroup = CommandGroupEnum.None)
        {
            await DeleteBotMessageByCommandGroupAsync(chatId, commandGroup, cancellationToken);

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

            await _botMessageService.AddRangeAsync(resultMessages.Select(x => new BotMessageModel()
            {
                MessageId = x.MessageId,
                ChatId = x.Chat.Id,
                CommandGroup = commandGroup
            }).ToList());

            return resultMessages.ToList();
        }

        public async Task<Message> EditTextMessageAsync(TextMessage message, long chatId, int messageId,
            CancellationToken cancellationToken = default)
        {
            return await _botClient.EditMessageTextAsync(chatId, messageId, message.Text,
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

            await _botMessageService.DeleteAsync(chatId, messageId);
        }

        private async Task DeleteBotMessageByCommandGroupAsync(long chatId, CommandGroupEnum commandGroup,
            CancellationToken cancellationToken = default)
        {
            if (commandGroup == CommandGroupEnum.None)
                return;

            var botMessages = await _botMessageService.GetRangeByCommandGroupAsync(chatId, commandGroup);

            var tasks = new List<Task>();
            foreach (var botMessage in botMessages)
            {
                tasks.Add(_botClient.DeleteMessageAsync(chatId, botMessage.MessageId, cancellationToken));
            }

            await Task.WhenAll(tasks);

            await _botMessageService.DeleteByCommandGroupAsync(chatId, commandGroup);
        }


    }
}
