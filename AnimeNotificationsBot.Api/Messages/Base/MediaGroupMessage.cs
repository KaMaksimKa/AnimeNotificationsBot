using AnimeNotificationsBot.Api.Models;

namespace AnimeNotificationsBot.Api.Messages.Base
{
    public class MediaGroupMessage : ITelegramMessage
    {
        public List<TelegramPhotoModel> Images { get; set; } = new List<TelegramPhotoModel>();
    }
}
