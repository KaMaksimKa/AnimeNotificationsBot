namespace AnimeNotificationsBot.Quartz.Configs
{
    public class QuartzConfig
    {
        public static readonly string Configuration = "QuartzConfig";

        public string AnimeNotificationCronSchedule { get; set; } = default!;
    }
}
