namespace AnimeNotificationsBot.Api.Services.Messages.Base
{
    public class PhotoMessage: TextMessage
    {
        public string? FileName { get; set; }
        public required Stream Content { get; set; } 
    }
}
