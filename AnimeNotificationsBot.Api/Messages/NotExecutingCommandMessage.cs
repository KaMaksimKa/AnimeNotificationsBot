using AnimeNotificationsBot.Api.Messages.Base;

namespace AnimeNotificationsBot.Api.Messages
{
    public class NotExecutingCommandMessage : TextMessage
    {
        public NotExecutingCommandMessage()
        {
            Text = "Никакого действия не выполнялось";
        }
    }
}
