namespace AnimeNotificationsBot.Quartz.Configs
{
    public class ParserConfig
    {
        public static readonly string Configuration = "ParserConfig";
        public string Cookies { get; set; } = default!;
        public string UserAgent { get; set; } = default!;
        public int TimeBetweenRequestFromSeconds { get; set; }
    }
}
