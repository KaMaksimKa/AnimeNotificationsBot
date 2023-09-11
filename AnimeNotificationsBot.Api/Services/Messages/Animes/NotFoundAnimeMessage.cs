using AnimeNotificationsBot.Api.Services.Messages.Base;

namespace AnimeNotificationsBot.Api.Services.Messages.Animes
{
    public class NotFoundAnimeMessage : TextMessage
    {
        public NotFoundAnimeMessage()
        {
            Text = "Аниме с таким названием я не нашел, можно попробывать поискать что-то другое";
        }
    }
}
