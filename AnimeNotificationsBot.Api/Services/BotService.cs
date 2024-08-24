using AnimeNotificationsBot.Api.Commands.Base.Args;
using AnimeNotificationsBot.Api.Messages.Notifications;
using AnimeNotificationsBot.Api.Services.Interfaces;
using AnimeNotificationsBot.BLL.Interfaces;
using Telegram.Bot.Types;

namespace AnimeNotificationsBot.Api.Services
{
    public class BotService:IBotService
    {
        private readonly ICommandFactory _commandFactory;
        private readonly IAnimeNotificationService _notificationService;
        private readonly IBotSender _botSender;

        public BotService(ICommandFactory commandFactory, IAnimeNotificationService notificationService, IBotSender botSender)
        {
            _commandFactory = commandFactory;
            _notificationService = notificationService;
            _botSender = botSender;
        }

        public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
        {
            try
            {
                var handler = update switch
                {
                    { Message: { } } => HandleMessageAsync(update.Message, cancellationToken),
                    { CallbackQuery: { } } => HandleCallbackQueryAsync(update.CallbackQuery, cancellationToken),
                    _ => DefaultHandleAsync(update, cancellationToken)
                };

                await handler;
            }
            catch(Exception ex) 
            {

            }
            
        }

        private async Task HandleMessageAsync(Message message, CancellationToken cancellationToken)
        {
            var command = _commandFactory.CreateMessageCommand(new MessageCommandArgs()
            {
                Message = message,
                CancellationToken = cancellationToken
            });

            await command.ExecuteAsync();
        }

        private async Task HandleCallbackQueryAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            var command = _commandFactory.CreateCallbackCommand(new CallbackCommandArgs
            {
                CallbackQuery = callbackQuery,
                CancellationToken = cancellationToken
            });

            await command.ExecuteAsync();
        }

        private Task DefaultHandleAsync(Update update, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task SendNotifications()
        {
            var notifications = await _notificationService.GetNewNotificationAsync();

            var tasks = new List<Task>();

            foreach (var notification in notifications)
            {
                foreach (var chatId in notification.ChatIds)
                {
                    tasks.Add(_botSender.SendMessageAsync(new NotifyAboutAnimeMessage(notification.AnimeNotificationModel), chatId));
                }
            }

            await Task.WhenAll(tasks);
        }
    }
}
