using AnimeNotificationsBot.Api.Services.Commands.TelegramCommands;
using AnimeNotificationsBot.Api.Services.Messages.Base;

namespace AnimeNotificationsBot.Api.Services.Messages.Feedbacks
{
    public class SendFeedbackMessage: TextMessage
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
