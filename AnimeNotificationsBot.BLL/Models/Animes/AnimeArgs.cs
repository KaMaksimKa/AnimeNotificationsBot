using AnimeNotificationsBot.BLL.Enums;

namespace AnimeNotificationsBot.BLL.Models.Animes
{
    public class AnimeArgs
    {
        public PaginationModel Pagination { get; set; } = new PaginationModel();
        public string? SearchQuery { get; set; }
        public AnimeSortTypeEnum SortType { get; set; } = AnimeSortTypeEnum.Rate;
        public AnimeSortOrderEnum SortOrder { get; set; } = AnimeSortOrderEnum.Desc;
        public bool OnlyOngoing { get; set; }

        public AnimeArgs Copy() => new AnimeArgs
        {
            Pagination = Pagination.Copy(),
            OnlyOngoing = OnlyOngoing,
            SearchQuery = SearchQuery,
            SortType = SortType,
            SortOrder = SortOrder
        };
    }
}
