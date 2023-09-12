using AnimeNotificationsBot.Api.Commands.TelegramCommands.Subscriptions;
using AnimeNotificationsBot.Api.Messages.Base;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.BLL.Models.Animes;
using Telegram.Bot.Types.ReplyMarkups;

namespace AnimeNotificationsBot.Api.Messages.Animes
{
    public class AnimeInfoMessage : PhotoMessage
    {
        public AnimeInfoMessage(AnimeInfoModel model, BackNavigationArgs backNavigationArgs, ICallbackQueryDataService callbackQueryDataService)
        {
            var anime = model.Anime;

            Text = $"""
                <b>{anime.TitleRu}</b>
                {(anime.NextEpisode != null ? $"\n🟢 {anime.NextEpisode}\n\n" : null)}Рейтинг - {anime.Rate}
                Количество серий - {anime.CountEpisode}

                В списках у людей: 
                 • Смотрю - {anime.Watching}  
                 • Просмотрено - {anime.Completed}  
                 • Брошено - {anime.Dropped}  
                 • Отложено - {anime.OnHold}
                 • Запланировано - {anime.Planned} 
                """;

            ParseMode = Telegram.Bot.Types.Enums.ParseMode.Html;

            ImgHref = anime.ImgHref;

            var buttons = new List<List<InlineKeyboardButton>>();

            if (model.ShowNotification)
            {
                buttons.Add(new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData("🔔Уведомления",SubscriptionDubbingByAnimeCommand.Create(anime.Id,callbackQueryDataService,backNavigationArgs.CurrCommandData).Result)
                });
            }

            if (backNavigationArgs.PrevCommandData != null)
                buttons.Add(new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("⬅️Назад", backNavigationArgs.PrevCommandData)
            });

            ReplyMarkup = new InlineKeyboardMarkup(buttons);
        }
    }
}
