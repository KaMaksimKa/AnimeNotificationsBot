namespace AnimeNotificationsBot.Api.Models
{
    public class CallbackData<TArgs>
    {
        public required TArgs Data { get; set; }
        public string? StringBackCommand { get; set; }
    }
}
