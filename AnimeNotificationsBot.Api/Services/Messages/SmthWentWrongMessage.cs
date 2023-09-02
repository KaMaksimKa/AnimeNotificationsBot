using AnimeNotificationsBot.Api.Services.Messages.Base;

namespace AnimeNotificationsBot.Api.Services.Messages
{
    public class SmthWentWrongMessage: TextMessage
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
