using AnimeNotificationsBot.Api.Services.Messages.Base;

namespace AnimeNotificationsBot.Api.Services.Messages
{
    public class CommandCancelledMessage:TextMessage
    {
        public CommandCancelledMessage()
        {
            Text = "Предыдущее действие отменено";
        }
    }
}
