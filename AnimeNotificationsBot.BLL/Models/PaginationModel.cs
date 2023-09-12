namespace AnimeNotificationsBot.BLL.Models
{
    public class PaginationModel
    {
        public int NumberOfPage { get; set; } = 1;
        public int CountPerPage { get; set; } = 5;

        public PaginationModel Copy() => new PaginationModel()
        {
            CountPerPage = CountPerPage,
            NumberOfPage = NumberOfPage
        };
    }
}
