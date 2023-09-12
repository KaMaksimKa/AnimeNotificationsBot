using AnimeNotificationsBot.Api.Messages.Base;

namespace AnimeNotificationsBot.Api.Messages.Feedbacks
{
    public class SentFeedbackMessage : TextMessage
    {
        public SentFeedbackMessage()
        {
            Text = "Большое спасибо за отзыв. Это поможет дальнейшей разработке";
        }
    }
}
