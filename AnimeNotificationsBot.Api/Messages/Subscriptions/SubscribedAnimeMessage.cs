using AnimeNotificationsBot.Api.Messages.Base;
using AnimeNotificationsBot.BLL.Models.Subscriptions;

namespace AnimeNotificationsBot.Api.Messages.Subscriptions
{
    public class SubscribedAnimeMessage : TextMessage
    {
        public SubscribedAnimeMessage(SubscribedAnimeModel model)
        {
            Text =
                $"Вы успешно подписались на рассылку уведомлений о выходе новых серий аниме {model.AnimeTitle} в озвучке {model.DubbingTitle}";
        }
    }
}
