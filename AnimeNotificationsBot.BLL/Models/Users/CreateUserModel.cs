namespace AnimeNotificationsBot.BLL.Models.Users
{
    public class CreateUserModel
    {
        public long TelegramChatId { get; set; }

        public long TelegramUserId { get; set; }

        public string FirstName { get; set; } = null!;

        public string? LastName { get; set; }

        public string? UserName { get; set; } = null!;
    }
}
