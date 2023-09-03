using AnimeNotificationsBot.Api.Services.Commands.Base.Args;
using AnimeNotificationsBot.Api.Services.Interfaces;
using Telegram.Bot.Types;

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

    }
}
