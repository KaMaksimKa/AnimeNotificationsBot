using AnimeNotificationsBot.Api.Services.Commands.TelegramCommands.Subscriptions;
using AnimeNotificationsBot.Api.Services.Messages.Base;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.BLL.Models.Animes;
using Telegram.Bot.Types.ReplyMarkups;

namespace AnimeNotificationsBot.Api.Services.Messages.Animes
{
    public class AnimeInfoMessage : PhotoMessage
    {
        public AnimeInfoMessage(AnimeInfoModel model, BackNavigationArgs backNavigationArgs, ICallbackQueryDataService callbackQueryDataService)
        {
            var anime = model.Anime;
            Text = anime.TitleRu;
            ImgHref = anime.ImgHref;

            var buttons = new List<List<InlineKeyboardButton>>();

            if (model.ShowNotification)
            {
                buttons.Add(new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData("Уведомления",СhoseAnimeForSubCommand.Create(anime.Id,callbackQueryDataService,backNavigationArgs.CurrCommandData).Result)
                });
            }
            
            if (backNavigationArgs.PrevCommandData != null)
                buttons.Add(new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("Назад", backNavigationArgs.PrevCommandData)
            });

            ReplyMarkup = new InlineKeyboardMarkup(buttons);
        }
    }
}
