namespace AnimeNotificationsBot.BLL.Models.Animes
{
    public class AnimeModel
    {
        public long Id { get; set; }
        public required string TitleRu { get; set; }
        public double? Rate { get; set; }
        public string? ImgHref { get; set; }
        public string? Href { get; set; }
    }
}
