namespace AnimeNotificationsBot.BLL.Models.Notifications
{
    public class AnimeNotificationWithChatsModel
    {
        public required AnimeNotificationModel AnimeNotificationModel { get; set; }
        public List<long> ChatIds { get; set; } = new List<long>();
    }
}
