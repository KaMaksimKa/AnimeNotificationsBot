using Telegram.Bot.Types;

namespace AnimeNotificationsBot.Api.Services.Commands.Base
{
    public class CallbackCommandArgs
    {
        public required CallbackQuery CallbackQuery { get; set; }
        public CancellationToken CancellationToken { get; set; }
    }
}
