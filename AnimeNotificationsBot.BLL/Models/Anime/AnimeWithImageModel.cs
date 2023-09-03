using AnimeNotificationsBot.Common.Models;

namespace AnimeNotificationsBot.BLL.Models.Anime
{
    public class AnimeWithImageModel: AnimeModel
    {
        public FileModel? Image { get; set; }
    }
}
