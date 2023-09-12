using AnimeNotificationsBot.Api.Messages.Base;

namespace AnimeNotificationsBot.Api.Messages
{
    public class SmthWentWrongMessage : TextMessage
    {
        public SmthWentWrongMessage(string? textError = null)
        {
            Text = "Упс... Что-то пошло не так, попробуйте позже.";

#if DEBUG
            Text += $"\nTextError: {textError}";
#endif
        }
    }
}
