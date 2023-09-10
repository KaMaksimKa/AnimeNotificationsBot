using AnimeNotificationsBot.BLL.Models.Animes;
using AnimeNotificationsBot.BLL.Models.Dubbing;

namespace AnimeNotificationsBot.BLL.Models.Notifications
{
    public class AnimeNotificationModel
    {
        public int? SerialNumber { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        public required AnimeModel Anime { get; set; }
        public required DubbingModel Dubbing { get; set; }
    }
}
