using AnimeNotificationsBot.Api.Services.Interfaces;
using Telegram.Bot.Types;

namespace AnimeNotificationsBot.Api.Services
{
    public class BotService:IBotService
    {
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
            
        }

        private async Task HandleCallbackQueryAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {

        }

        private async Task DefaultHandleAsync(Update update, CancellationToken cancellationToken)
        {

        }
    }
}
