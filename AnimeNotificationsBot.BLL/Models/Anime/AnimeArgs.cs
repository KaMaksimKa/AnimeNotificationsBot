using AnimeNotificationsBot.BLL.Enums;

namespace AnimeNotificationsBot.BLL.Models.Anime
{
    public class AnimeArgs
    {
        public int NumberOfPage { get; set; } = 1;
        public int CountPerPage { get; set; } = 5;
        public string? SearchQuery { get; set; }
        public AnimeSortTypeEnum SortType { get; set; } = AnimeSortTypeEnum.Rate;
        public AnimeSortOrderEnum SortOrder { get; set; } = AnimeSortOrderEnum.Desc;
    }
}
