using Telegram.Bot.Types;

namespace AnimeNotificationsBot.Api.Services.Interfaces
{
    public interface IBotService
    {
        public Task HandleUpdateAsync(Update update,CancellationToken cancellationToken);
    }
}
