namespace AnimeNotificationsBot.BLL.Models.Subscription
{
    public class SubscribedAnimeModel
    {
        public long? AnimeId { get; set; }
        public string? AnimeTitle { get; set; }

        public long? DubbingId { get; set; }
        public string? DubbingTitle { get;set; }


        public int SubscriptionsCount { get; set; }
    }
}
