using AnimeNotificationsBot.Api.Models;
using AnimeNotificationsBot.Api.Services.Messages.Base;
using AnimeNotificationsBot.BLL.Models.Anime;

namespace AnimeNotificationsBot.Api.Services.Messages.Anime
{
    public class AnimeImagesMessage:MediaGroupMessage
    {
        public AnimeImagesMessage(List<AnimeWithImageModel> animes)
        {
            Images = animes
                .Where(x => x.Image != null)
                .Select(x => new TelegramPhotoModel()
                {
                    Caption = x.TitleRu,
                    Image = x.Image!
                }).ToList();
        }
    }
}
