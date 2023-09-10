using AnimeNotificationsBot.Api.Services.Messages.Base;

namespace AnimeNotificationsBot.Api.Services.Messages
{
    public class NotExecutingCommandMessage: TextMessage
    {
        public NotExecutingCommandMessage()
        {
            Text = "Никакого действия не выполнялось";
        }
    }
}
