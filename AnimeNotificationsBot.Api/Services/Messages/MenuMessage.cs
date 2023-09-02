using AnimeNotificationsBot.Api.Services.Commands;
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
                new []{new KeyboardButton(HelpCommand.CreateFriendly()) }
            })
            {
                ResizeKeyboard = true
            };
            ParseMode = helpMessage.ParseMode;
        }
    }
}
