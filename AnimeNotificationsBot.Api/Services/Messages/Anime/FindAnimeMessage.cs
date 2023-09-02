using AnimeNotificationsBot.Api.Services.Messages.Base;

namespace AnimeNotificationsBot.Api.Services.Messages.Anime
{
    public class FindAnimeMessage: TextMessage
    {
        public FindAnimeMessage()
        {
            Text = "Отправьте мне название аниме для поиска.";
        }
    }
}
