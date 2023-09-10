using AnimeNotificationsBot.Api.Models;
using AnimeNotificationsBot.Api.Services.Messages.Base;
using AnimeNotificationsBot.BLL.Models.Animes;

namespace AnimeNotificationsBot.Api.Services.Messages.Animes
{
    public class AnimeImagesMessage:MediaGroupMessage
    {
        public AnimeImagesMessage(List<AnimeModel> animes)
        {
            Images = animes
                .Where(x => x.ImgHref != null)
                .Select(x => new TelegramPhotoModel()
                {
                    Caption = x.TitleRu,
                    ImgHref = x.ImgHref,
                }).ToList();
        }
    }
}
