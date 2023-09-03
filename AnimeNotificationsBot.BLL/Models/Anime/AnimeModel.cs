namespace AnimeNotificationsBot.BLL.Models.Anime
{
    public class AnimeModel
    {
        public long Id { get; set; }
        public required string TitleRu { get; set; }
        public required double? Rate { get; set; }
    }
}
