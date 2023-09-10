namespace AnimeNotificationsBot.BLL.Models.Animes
{
    public class AnimeListModel
    {
        public AnimeArgs Args { get; set; } = null!;
        public int CountAllAnime { get; set; }
        public int CountPages => (int)Math.Ceiling((double)CountAllAnime / Args.Pagination.CountPerPage);
        public List<AnimeModel> Animes { get; set; } = null!;
    }
}
