namespace AnimeNotificationsBot.BLL.Models.Animes
{
    public class AnimeInfoModel
    {
        public required AnimeModel Anime { get; set; }
        public bool ShowNotification { get; set; }
    }
}
