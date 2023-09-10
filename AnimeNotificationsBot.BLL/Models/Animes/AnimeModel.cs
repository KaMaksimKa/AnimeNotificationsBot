namespace AnimeNotificationsBot.BLL.Models.Animes
{
    public class AnimeModel
    {
        public long Id { get; set; }
        public required string TitleRu { get; set; }
        public double? Rate { get; set; }
        public string? ImgHref { get; set; }
        public string? Href { get; set; }
        public int? CountEpisode { get; set; }
        public int? Planned { get; set; }
        public int? Completed { get; set; }
        public int? Watching { get; set; }
        public int? Dropped { get; set; }
        public int? OnHold { get; set; }
        public string? NextEpisode { get; set; }
        public bool IsOngoing { get; set; }
    }
}
