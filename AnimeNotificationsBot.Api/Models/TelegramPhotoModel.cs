using AnimeNotificationsBot.Common.Models;

namespace AnimeNotificationsBot.Api.Models
{
    public class TelegramPhotoModel
    {
        public string? Caption { get; set; }
        public required string? ImgHref { get; set;}
    }
}
