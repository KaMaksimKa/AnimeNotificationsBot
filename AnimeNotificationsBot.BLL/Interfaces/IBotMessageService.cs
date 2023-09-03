using AnimeNotificationsBot.BLL.Models.BotMessage;
using AnimeNotificationsBot.Common.Enums;

namespace AnimeNotificationsBot.BLL.Interfaces
{
    public interface IBotMessageService
    {
        Task AddAsync(BotMessageModel model);
        Task AddRangeAsync(List<BotMessageModel> models);
        Task DeleteAsync(long chatId, int messageId);
        Task DeleteByCommandGroupAsync(long chatId, CommandGroupEnum commandGroup);
        Task<List<BotMessageModel>> GetRangeByCommandGroupAsync(long chatId, CommandGroupEnum commandGroup);
    }
}
