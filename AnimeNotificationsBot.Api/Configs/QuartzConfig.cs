namespace AnimeNotificationsBot.Api.Configs
{
    public class QuartzConfig
    {
        public static readonly string Configuration = "QuartzConfig";

        public string NotifyAboutAnimeJobCronSchedule { get; init; } = default!;
    }
}
