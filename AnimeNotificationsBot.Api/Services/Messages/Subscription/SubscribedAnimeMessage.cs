using AnimeNotificationsBot.Api.Services.Messages.Base;
using AnimeNotificationsBot.BLL.Models.Subscription;

namespace AnimeNotificationsBot.Api.Services.Messages.Subscription
{
    public class SubscribedAnimeMessage: TextMessage
    {
        public SubscribedAnimeMessage(SubscribedAnimeModel model)
        {
            Text =
                $"Вы успешно подписались на рассылку уведомлений о выходе новых серий аниме {model.AnimeTitle} в озвучке {model.DubbingTitle}";
        }
    }
}
