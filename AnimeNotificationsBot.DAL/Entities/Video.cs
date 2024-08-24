using AnimeNotificationsBot.Common.Interfaces;

namespace AnimeNotificationsBot.DAL.Entities
{
    public class Video : IEntity
    {
        public long Id { get; set; }
        public string Quality { get; set; } = null!;
        public string ManifestLink { get; set; } = null!;
        public long? MediaDocumentId { get; set; }

        public long VideoInfoId { get; set; }
        public VideoInfo VideoInfo { get; set; } = null!;
    }
}
