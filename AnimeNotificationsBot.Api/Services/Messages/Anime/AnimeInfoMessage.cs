using AnimeNotificationsBot.Api.Services.Messages.Base;
using AnimeNotificationsBot.BLL.Models.Anime;

namespace AnimeNotificationsBot.Api.Services.Messages.Anime
{
    public class AnimeInfoMessage: PhotoMessage
    {
        public AnimeInfoMessage(AnimeWithImageModel anime)
        {
            Text = anime.TitleRu;
            Photo = anime.Image;
        }
    }
}
