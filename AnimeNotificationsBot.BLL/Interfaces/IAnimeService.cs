using AnimeNotificationsBot.BLL.Models.Animes;

namespace AnimeNotificationsBot.BLL.Interfaces
{
    public interface IAnimeService
    {
        Task<AnimeModel> GetAnimeWithImageAsync(long id);

        Task<AnimeListModel> GetAnimeWithImageByArgsAsync(AnimeArgs args);

        Task<AnimeInfoModel> GetAnimeInfoModel(long animeId);
    }
}
