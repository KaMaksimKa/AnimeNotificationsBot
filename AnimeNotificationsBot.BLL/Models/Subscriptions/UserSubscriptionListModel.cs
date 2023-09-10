using AnimeNotificationsBot.BLL.Models.Animes;

namespace AnimeNotificationsBot.BLL.Models.Subscriptions
{
    public class UserSubscriptionListModel
    {
        public required PaginationModel Pagination { get; set; }
        public int CountAllAnime { get; set; }
        public int CountPages => (int)Math.Ceiling((double)CountAllAnime / Pagination.CountPerPage);
        public List<AnimeModel> Animes { get; set; } = null!;
    }
}
