using AnimeNotificationsBot.Api.Services.Commands.TelegramCommands.Animes;
using AnimeNotificationsBot.Api.Services.Messages.Base;
using AnimeNotificationsBot.BLL.Enums;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.BLL.Models;
using AnimeNotificationsBot.BLL.Models.Animes;
using Telegram.Bot.Types.ReplyMarkups;

namespace AnimeNotificationsBot.Api.Services.Messages.Animes
{
    public class AnimeListMessage: CombiningMessage
    {
        private const int MaxCountPageOnMessage = 5;
        public AnimeListMessage(AnimeListModel model, ICallbackQueryDataService callbackQueryDataService,BackNavigationArgs backNavigationArgs)
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
                Страница {model.Args.Pagination.NumberOfPage} из {model.CountPages}.
                Сорнировка по {sort} в порядке {order}.
                """;


            var buttons = new List<List<InlineKeyboardButton>>();
            foreach (var anime in model.Animes)
            {
                buttons.Add(new List<InlineKeyboardButton>()
                {
                    InlineKeyboardButton.WithCallbackData(anime.TitleRu,AnimeInfoCommand.Create(anime.Id,callbackQueryDataService,backNavigationArgs.CurrCommandData).Result)
                });
            }

            if (model.Animes.Count > MaxCountPageOnMessage)
            {
                var lineWithNumberOfPages = new List<InlineKeyboardButton>();

                var start = model.Args.Pagination.NumberOfPage - MaxCountPageOnMessage / 2;
                if (start < 1)
                    start = 1;
                else if (start + MaxCountPageOnMessage > model.CountPages)
                    start = Math.Max(1, model.CountPages - MaxCountPageOnMessage + 1);

                var finish = Math.Min(model.CountPages + 1, start + MaxCountPageOnMessage);
                for (int numberOfPage = start; numberOfPage < finish; numberOfPage++)
                {
                    var animeArgsForPage = new AnimeArgs()
                    {
                        SortOrder = model.Args.SortOrder,
                        SortType = model.Args.SortType,
                        Pagination = new PaginationModel()
                        {
                            CountPerPage = model.Args.Pagination.CountPerPage,
                            NumberOfPage = numberOfPage
                        },
                        SearchQuery = model.Args.SearchQuery,
                    };

                    var nameButtonPage = numberOfPage == model.Args.Pagination.NumberOfPage ? $"✅{numberOfPage}" : numberOfPage.ToString();

                    lineWithNumberOfPages.Add(InlineKeyboardButton.WithCallbackData(nameButtonPage, AnimeListCommand.Create(animeArgsForPage, callbackQueryDataService).Result));
                }

                buttons.Add(lineWithNumberOfPages);
            }
            
            textMessage.ReplyMarkup = new InlineKeyboardMarkup(buttons);

            Messages.Add(animeImagesMessage);
            Messages.Add(textMessage);
        }
    }
}
