namespace AnimeNotificationsBot.Quartz.Configs
{
    public class RequestParserFactoryConfig
    {
        public static readonly string Configuration = "RequestParserFactoryConfig";
        public string Cookies { get; set; } = default!;
        public string UserAgent { get; set; } = default!;
    }
}
