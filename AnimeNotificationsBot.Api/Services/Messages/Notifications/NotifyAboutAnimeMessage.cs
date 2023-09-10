using AnimeNotificationsBot.Api.Services.Messages.Base;
using AnimeNotificationsBot.BLL.Models.Animes;
using AnimeNotificationsBot.BLL.Models.Notifications;
using Telegram.Bot.Types.ReplyMarkups;

namespace AnimeNotificationsBot.Api.Services.Messages.Notifications
{
    public class NotifyAboutAnimeMessage:PhotoMessage
    {
        public NotifyAboutAnimeMessage(AnimeNotificationModel notification)
        {
            ImgHref = notification.Anime.ImgHref;
            Text =
                $"Вышла {notification.SerialNumber} серия аниме {notification.Anime.TitleRu} в озвучке {notification.Dubbing.Title}";

            if (notification.Anime.Href != null)
            {
                ReplyMarkup = new InlineKeyboardMarkup(InlineKeyboardButton.WithUrl("AnimeGo",notification.Anime.Href));
            }
        }
    }
}
