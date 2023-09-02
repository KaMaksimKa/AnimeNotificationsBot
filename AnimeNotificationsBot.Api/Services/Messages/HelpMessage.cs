using AnimeNotificationsBot.Api.Services.Messages.Base;

namespace AnimeNotificationsBot.Api.Services.Messages
{
    public class HelpMessage: TextMessage
    {
        public HelpMessage()
        {
            Text = "help";
        }
    }
}
