using Telegram.Bot.Types;

namespace AnimeNotificationsBot.Api.Services.Commands.Base
{
    public class MessageCommandArgs
    {
        public required Message Message { get; set; }
        public CancellationToken CancellationToken { get; set; }
    }
}
