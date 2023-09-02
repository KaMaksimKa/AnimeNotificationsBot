using AnimeNotificationsBot.Common.Models;

namespace AnimeNotificationsBot.BLL.Models.Anime
{
    public class AnimeImageModel
    {
        public string? AnimeName { get; set; }
        public required FileModel Image { get; set; }
    }
}
