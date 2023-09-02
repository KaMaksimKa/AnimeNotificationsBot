using AnimeNotificationsBot.BLL.Models.Anime;

namespace AnimeNotificationsBot.BLL.Interfaces
{
    public interface IAnimeService
    {
        Task<List<AnimeWithImageModel>> GetAnimesWithImagesAsync(string? searchQuery = null, int numberOfPage = 1,
            int quantity = 5);

        Task<AnimeWithImageModel> GetAnimeWithImageAsync(long id);
    }
}
