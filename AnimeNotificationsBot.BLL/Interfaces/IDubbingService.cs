using AnimeNotificationsBot.BLL.Models.Dubbing;

namespace AnimeNotificationsBot.BLL.Interfaces
{
    public interface IDubbingService
    {
        public Task<List<DubbingModel>> GetDubbingByAnimeIdAsync(long animeId);
    }
}
