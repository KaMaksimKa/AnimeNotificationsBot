using AnimeNotificationsBot.Common.Models;

namespace AnimeNotificationsBot.Api.Services.Messages.Base
{
    public class PhotoMessage: TextMessage
    {
        public string? ImgHref { get; set; }
    }
}
