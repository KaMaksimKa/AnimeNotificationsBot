using AnimeNotificationsBot.Api.Commands.TelegramCommands;
using AnimeNotificationsBot.Api.Commands.TelegramCommands.Animes;
using AnimeNotificationsBot.Api.Commands.TelegramCommands.Feedbacks;
using AnimeNotificationsBot.Api.Commands.TelegramCommands.Subscriptions;
using AnimeNotificationsBot.Api.Messages.Base;
using Telegram.Bot.Types.ReplyMarkups;

namespace AnimeNotificationsBot.Api.Messages
{
    public class MenuMessage : TextMessage
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
