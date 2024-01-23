using AnimeNotificationsBot.Common.Interfaces;

namespace AnimeNotificationsBot.DAL.Entities
{
    public class VideoProvider: IEntity,IHasUniqueProperty
    {
        public long Id { get; set; }
        public required string Name { get; set; }

        public virtual List<VideoInfo> VideoInfos { get; set; } = new List<VideoInfo>();

        public object UniqueProperty => Name;
    }
}
