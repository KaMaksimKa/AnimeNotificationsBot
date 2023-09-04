using AnimeNotificationsBot.Api.Services.Messages.Base;
using AnimeNotificationsBot.BLL.Models.Anime;
using Telegram.Bot.Types.ReplyMarkups;

namespace AnimeNotificationsBot.Api.Services.Messages.Anime
{
    public class AnimeInfoMessage: PhotoMessage
    {
        public AnimeInfoMessage(AnimeWithImageModel anime, BackNavigationArgs backNavigationArgs)
        {
            Text = anime.TitleRu;
            Photo = anime.Image;

            var buttons = new List<List<InlineKeyboardButton>>();

            buttons.Add(new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("Подписаться")
            });

            if (backNavigationArgs.ChildrenBackData != null) 
                buttons.Add(new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("Назад", backNavigationArgs.ChildrenBackData)
            });

            ReplyMarkup = new InlineKeyboardMarkup(buttons);
        }
    }
}
