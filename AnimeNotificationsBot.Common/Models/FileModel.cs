namespace AnimeNotificationsBot.Common.Models
{
    public class FileModel
    {
        public required string FileName { get; set; }
        public required Stream Content { get; set; }
    }
}
