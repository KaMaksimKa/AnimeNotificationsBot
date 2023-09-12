using AnimeNotificationsBot.Api.Commands.TelegramCommands;
using AnimeNotificationsBot.Api.Messages.Base;

namespace AnimeNotificationsBot.Api.Messages.Animes
{
    public class FindAnimeMessage : TextMessage
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
