namespace AnimeNotificationsBot.Api.Models
{
    public class CallbackDataModel<TArgs>
    {
        public required TArgs Data { get; set; }
        public string? PrevStringCommand { get; set; }
    }
}
