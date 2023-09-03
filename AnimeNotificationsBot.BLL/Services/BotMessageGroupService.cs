using AnimeNotificationsBot.BLL.Helpers;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.BLL.Models.BotMessageGroup;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace AnimeNotificationsBot.BLL.Services
{
    public class BotMessageGroupService: IBotMessageGroupService
    {
        private readonly DataContext _context;

        public BotMessageGroupService(DataContext context)
        {
            _context = context;
        }

        public async Task AddAsync(BotMessageGroupModel model)
        {
            var user = await _context.Users.GetUserByChatId(model.ChatId);

            await _context.BotMessageGroups.AddAsync(new BotMessageGroup()
            {
                User = user,
                Messages = model.MessageIds.Select(x => new BotMessage()
                {
                    MessageId = x
                }).ToList()
            });

            await _context.SaveChangesAsync();
        }

        public async Task<BotMessageGroupModel?> GetByMessageIdAsync(long telegramMessageId)
        {
            return await _context.BotMessageGroups
                .Where(x => x.Messages.Any(y => y.MessageId == telegramMessageId))
                .Select(x => new BotMessageGroupModel
                {
                    ChatId = x.User.TelegramChatId,
                    MessageIds = x.Messages.Select(y => y.MessageId).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task DeleteByMessageIdAsync(long telegramMessageId)
        {
            throw new NotImplementedException();
        }
    }
}
