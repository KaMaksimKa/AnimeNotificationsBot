using AnimeNotificationsBot.BLL.Models.BotMessage;
using AnimeNotificationsBot.BLL.Models.BotMessageGroup;

namespace AnimeNotificationsBot.BLL.Interfaces
{
    public interface IBotMessageGroupService
    {
        public Task AddAsync(BotMessageGroupModel model);

        public Task<BotMessageGroupModel?> GetByMessageIdAsync(long telegramMessageId);

        public Task DeleteByMessageIdAsync(long telegramMessageId);

    }
}
