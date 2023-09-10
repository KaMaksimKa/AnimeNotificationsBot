namespace AnimeNotificationsBot.Api.Services.Messages.Base
{
    public class CombiningMessage:ITelegramMessage
    {
        public List<ITelegramMessage> Messages { get; set; } = new List<ITelegramMessage>();
    }
}
