using AnimeNotificationsBot.Api.Services.Commands.TelegramCommands;
using AnimeNotificationsBot.Api.Services.Messages.Base;

namespace AnimeNotificationsBot.Api.Services.Messages.Animes
{
    public class FindAnimeMessage: TextMessage
    {
        public FindAnimeMessage()
        {
            Text = $"""
                Отправьте мне название аниме для поиска
    
                {CancelCommand.Create()} - отменить действие
                """;
        }
    }
}
