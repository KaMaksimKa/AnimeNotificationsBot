using AnimeNotificationsBot.Api.Services.Messages.Base;

namespace AnimeNotificationsBot.Api.Services.Messages.Feedbacks
{
    public class SentFeedbackMessage: TextMessage
    {
        public SentFeedbackMessage()
        {
            Text = "Большое спасибо за отзыв. Это поможет дальнейшей разработке";
        }
    }
}
