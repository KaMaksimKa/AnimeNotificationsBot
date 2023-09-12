using AnimeNotificationsBot.Api.Messages.Base;

namespace AnimeNotificationsBot.Api.Messages
{
    public class CommandCancelledMessage : TextMessage
    {
        public CommandCancelledMessage()
        {
            Text = "Предыдущее действие отменено";
        }
    }
}
