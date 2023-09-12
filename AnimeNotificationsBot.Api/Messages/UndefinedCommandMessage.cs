using AnimeNotificationsBot.Api.Messages.Base;

namespace AnimeNotificationsBot.Api.Messages
{
    public class UndefinedCommandMessage : TextMessage
    {
        public UndefinedCommandMessage()
        {
            Text = "Что-то я вас не понимаю, попробуйте ввести что-то другое...";
        }
    }
}
