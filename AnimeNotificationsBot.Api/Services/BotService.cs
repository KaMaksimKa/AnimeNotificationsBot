using AnimeNotificationsBot.Api.Services.Commands.Base;
using AnimeNotificationsBot.Api.Services.Interfaces;
using System.Threading;
using Telegram.Bot.Types;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AnimeNotificationsBot.Api.Services
{
    public class BotService:IBotService
    {
        private readonly ICommandFactory _commandFactory;

        public BotService(ICommandFactory commandFactory)
        {
            _commandFactory = commandFactory;
        }

        public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
        {
            var handler = update switch
            {
                { Message: { } } => HandleMessageAsync(update.Message, cancellationToken),
                { CallbackQuery: { } } => HandleCallbackQueryAsync(update.CallbackQuery, cancellationToken),
                _ => DefaultHandleAsync(update, cancellationToken)
            };

            await handler;
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

        private async Task DefaultHandleAsync(Update update, CancellationToken cancellationToken)
        {

        }

    }
}
