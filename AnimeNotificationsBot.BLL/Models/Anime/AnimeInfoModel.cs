namespace AnimeNotificationsBot.BLL.Models.Anime
{
    public class AnimeInfoModel
    {
        public required AnimeWithImageModel Anime { get; set; }
        public bool ShowNotification { get; set; }
    }
}
