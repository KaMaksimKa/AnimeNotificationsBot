using AnimeNotificationsBot.Common.Models;

namespace AnimeNotificationsBot.Api.Messages.Base
{
    public class PhotoMessage : TextMessage
    {
        public string? ImgHref { get; set; }
    }
}
