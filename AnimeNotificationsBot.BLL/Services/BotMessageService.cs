using AnimeNotificationsBot.BLL.Helpers;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.BLL.Models.BotMessage;
using AnimeNotificationsBot.Common.Enums;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace AnimeNotificationsBot.BLL.Services
{
    public class BotMessageService: IBotMessageService
    {
        private readonly DataContext _context;

        public BotMessageService(DataContext context)
        {
            _context = context;
        }

        public async Task AddAsync(BotMessageModel model)
        {
            await AddBotMessageModel(model);

            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(List<BotMessageModel> models)
        {
            foreach (var model in models)
            {
                await AddBotMessageModel(model);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long chatId, int messageId)
        {
            var botMessage =await _context.BotMessages
                .Where(x => x.User.TelegramChatId == chatId && x.MessageId == messageId)
                .FirstOrDefaultAsync();

            if (botMessage != null)
            {
                botMessage.IsRemoved = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<BotMessageModel>> GetRangeByCommandGroupAsync(long chatId, CommandGroupEnum commandGroup)
        {
            return await _context.BotMessages
                .Where(x => !x.IsRemoved && x.User.TelegramChatId == chatId && x.CommandGroup == commandGroup)
                .Select(x => new BotMessageModel()
                {
                    MessageId = x.MessageId,
                    CommandGroup = commandGroup,
                    ChatId = x.User.TelegramChatId
                })
                .ToListAsync();
        }

        private async Task AddBotMessageModel(BotMessageModel model)
        {
            var user = await _context.Users.GetUserByChatId(model.ChatId);
            await _context.BotMessages.AddAsync(new BotMessage()
            {
                MessageId = model.MessageId,
                CommandGroup = model.CommandGroup,
                User = user
            });
        }
    }
}
