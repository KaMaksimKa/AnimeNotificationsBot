namespace AnimeNotificationsBot.BLL.Models.Subscriptions
{
    public class SubscribeAnimeModel
    {
        /// <summary>
        /// Если null, то подразумеваются все аниме
        /// </summary>
        public long? AnimeId { get; set; }

        /// <summary>
        /// Если null, то подразумеваются все озвучки
        /// </summary>
        public long? DubbingId { get; set; }
    }
}
