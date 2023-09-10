using AnimeNotificationsBot.Api.Services.Commands.TelegramCommands;
using AnimeNotificationsBot.Api.Services.Commands.TelegramCommands.Animes;
using AnimeNotificationsBot.Api.Services.Commands.TelegramCommands.Feedbacks;
using AnimeNotificationsBot.Api.Services.Commands.TelegramCommands.Subscriptions;
using AnimeNotificationsBot.Api.Services.Messages.Base;
using Telegram.Bot.Types.ReplyMarkups;

namespace AnimeNotificationsBot.Api.Services.Messages
{
    public class MenuMessage: TextMessage
    {
        public MenuMessage()
        {
            var helpMessage = new HelpMessage();
            Text = helpMessage.Text;
            ReplyMarkup = new ReplyKeyboardMarkup(new KeyboardButton[][]{
                new []
                {
                    new KeyboardButton(FindAnimeCommand.CreateFriendly()),
                    new KeyboardButton(HelpCommand.CreateFriendly())
                },
                new []
                {
                    new KeyboardButton(UserSubscriptionsCommand.CreateFriendly()),
                    new KeyboardButton(SendFeedbackCommand.CreateFriendly())
                }
            })
            {
                ResizeKeyboard = true
            };
            ParseMode = helpMessage.ParseMode;
        }
    }
}
