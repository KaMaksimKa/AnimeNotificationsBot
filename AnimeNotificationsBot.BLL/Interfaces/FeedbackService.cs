namespace AnimeNotificationsBot.BLL.Interfaces
{
    public interface IFeedbackService
    {
        public Task AddAsync(string message, long telegramUserId);
    }
}
