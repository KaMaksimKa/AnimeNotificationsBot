using AnimeNotificationsBot.Api.Commands.TelegramCommands;
using AnimeNotificationsBot.Api.Messages.Base;

namespace AnimeNotificationsBot.Api.Messages.Feedbacks
{
    public class SendFeedbackMessage : TextMessage
    {
        public SendFeedbackMessage()
        {
            Text = $"""
                Оставте отзыв, пожелания или жалобу для данного бота, и это повлияет на дальнейшие развитие AnimeGoNotificationBot 
    
                {CancelCommand.Create()} - отменить действие
                """;
        }
    }
}
