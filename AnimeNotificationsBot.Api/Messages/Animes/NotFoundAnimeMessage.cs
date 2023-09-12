using AnimeNotificationsBot.Api.Messages.Base;

namespace AnimeNotificationsBot.Api.Messages.Animes
{
    public class NotFoundAnimeMessage : TextMessage
    {
        public NotFoundAnimeMessage()
        {
            Text = "Аниме с таким названием я не нашел, можно попробывать поискать что-то другое";
        }
    }
}
