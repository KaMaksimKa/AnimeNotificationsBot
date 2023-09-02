using AnimeNotificationsBot.Api.Services.Messages.Base;

namespace AnimeNotificationsBot.Api.Services.Messages
{
    public class UndefinedCommandMessage : TextMessage
    {
        public UndefinedCommandMessage()
        {
            Text = "Что-то я вас не понимаю, попробуйте ввести что-то другое...";
        }
    }
}
