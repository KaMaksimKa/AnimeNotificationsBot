using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AnimeNotificationsBot.BLL.Models.Anime
{
    public class AnimeListModel
    {
        public AnimeArgs Args { get; set; } = null!;
        public int CountAllAnime { get; set; }
        public int CountPages => (int)Math.Ceiling((double)CountAllAnime / Args.CountPerPage);
        public List<AnimeWithImageModel> Animes { get; set; } = null!;
    }
}
