using AnimeNotificationsBot.BLL.Models.Anime;

namespace AnimeNotificationsBot.BLL.Interfaces
{
    public interface IAnimeService
    {
        Task<AnimeWithImageModel> GetAnimeWithImageAsync(long id);

        Task<AnimeListModel> GetAnimeWithImageByArgsAsync(AnimeArgs args);

        Task<AnimeInfoModel> GetAnimeInfoModel(long animeId);
    }
}
