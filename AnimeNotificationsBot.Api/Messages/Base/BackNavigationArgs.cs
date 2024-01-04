namespace AnimeNotificationsBot.Api.Messages.Base
{
    public class BackNavigationArgs
    {
        public string? PrevCommandData { get; set; }
        public required string CurrCommandData { get; set; }
    }
}
