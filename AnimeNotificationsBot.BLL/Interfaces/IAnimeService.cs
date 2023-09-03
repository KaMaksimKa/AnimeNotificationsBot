using AnimeNotificationsBot.BLL.Models.Anime;

namespace AnimeNotificationsBot.BLL.Interfaces
{
    public interface IAnimeService
    {
        Task<List<AnimeWithImageModel>> GetAnimesWithImagesAsync(string? searchQuery = null);

        Task<AnimeWithImageModel> GetAnimeWithImageAsync(long id);

        Task<AnimeListModel> GetAnimeWithImageByArgsAsync(AnimeArgs args);
    }
}
