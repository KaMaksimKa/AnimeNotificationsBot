using AnimeNotificationsBot.Api.Services.Commands.TelegramCommands.Anime;
using AnimeNotificationsBot.Api.Services.Messages.Base;
using AnimeNotificationsBot.BLL.Enums;
using AnimeNotificationsBot.BLL.Models.Anime;
using AnimeNotificationsBot.DAL.Entities;
using System.Xml.Linq;
using AnimeNotificationsBot.BLL.Interfaces;
using Telegram.Bot.Types.ReplyMarkups;

namespace AnimeNotificationsBot.Api.Services.Messages.Anime
{
    public class AnimeListMessage: CombiningMessage
    {
        private const int MaxCountPageOnMessage = 7;
        public AnimeListMessage(AnimeListModel model, ICallbackQueryDataService callbackQueryDataService)
        {
            var animeImagesMessage = new AnimeImagesMessage(model.Animes);

            var textMessage = new TextMessage();

            var sort = model.Args.SortType switch
            {
                AnimeSortTypeEnum.Rate => "рейтингу",
                AnimeSortTypeEnum.Name => "имени",
            };

            var order = model.Args.SortOrder switch
            {
                AnimeSortOrderEnum.Asc => "возрастания",
                AnimeSortOrderEnum.Desc => "убывания",
            };

            textMessage.Text = $"""
                Всего Аниме найдено - {model.CountAllAnime}.
                Страница {model.Args.NumberOfPage} из {model.CountPage}.
                Сорнировка по {sort} в порядке {order}.
                """;


            var buttons = new List<List<InlineKeyboardButton>>();
            foreach (var anime in model.Animes)
            {
                buttons.Add(new List<InlineKeyboardButton>()
                {
                    InlineKeyboardButton.WithCallbackData(anime.TitleRu,AnimeInfoCommand.Create(anime.Id,callbackQueryDataService).Result)
                });
            }

            var lineWithNumberOfPages = new List<InlineKeyboardButton>();
            var start = Math.Max(1, model.Args.NumberOfPage-MaxCountPageOnMessage/2);
            var finish = Math.Min(start + model.CountPage, start + MaxCountPageOnMessage);
            for (int numberOfPage = start; numberOfPage < finish; numberOfPage++)
            {
                var animeArgsForPage = new AnimeArgs()
                {
                    SortOrder = model.Args.SortOrder,
                    SortType = model.Args.SortType,
                    CountPerPage = model.Args.CountPerPage,
                    NumberOfPage = numberOfPage,
                    SearchQuery = model.Args.SearchQuery,
                };

                var nameButtonPage = numberOfPage == model.Args.NumberOfPage?$"✅{numberOfPage}" :numberOfPage.ToString();

                lineWithNumberOfPages.Add(InlineKeyboardButton.WithCallbackData(nameButtonPage, AnimeListCommand.Create(animeArgsForPage, callbackQueryDataService).Result));
            }

            buttons.Add(lineWithNumberOfPages);

            textMessage.ReplyMarkup = new InlineKeyboardMarkup(buttons);

            Messages.Add(animeImagesMessage);
            Messages.Add(textMessage);
        }
    }
}
