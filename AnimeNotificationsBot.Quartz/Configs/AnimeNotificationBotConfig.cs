namespace AnimeNotificationsBot.Quartz.Configs
{
    public class AnimeNotificationBotConfig
    {
        public const string Configuration = "AnimeNotificationBotConfig";
        public string Hostname { get; set; } = null!;
        public string SendNotificationUrl { get; set; } = null!;

    }
}
