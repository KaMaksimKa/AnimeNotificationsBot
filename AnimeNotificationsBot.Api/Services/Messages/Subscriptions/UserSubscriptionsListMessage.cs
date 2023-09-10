using AnimeNotificationsBot.Api.Services.Commands.TelegramCommands.Subscriptions;
using AnimeNotificationsBot.Api.Services.Messages.Animes;
using AnimeNotificationsBot.Api.Services.Messages.Base;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.BLL.Models;
using AnimeNotificationsBot.BLL.Models.Animes;
using AnimeNotificationsBot.BLL.Models.Subscriptions;
using AnimeNotificationsBot.BLL.Services;
using Telegram.Bot.Types.ReplyMarkups;

namespace AnimeNotificationsBot.Api.Services.Messages.Subscriptions
{
    public class UserSubscriptionsListMessage: CombiningMessage
    {
        private int MaxCountPageOnMessage = 5;
        public UserSubscriptionsListMessage(UserSubscriptionListModel model, BackNavigationArgs backNavigationArgs,
            ICallbackQueryDataService callbackQueryDataService)
        {
            var mediaGroupMessage = new AnimeImagesMessage(model.Animes);
            var textMessage = new TextMessage();

            textMessage.Text = "Список тайтлов на которые ты подписан: <i>(стр. 1 из 1)</i>";
            textMessage.ParseMode = Telegram.Bot.Types.Enums.ParseMode.Html;

            var buttons = new List<List<InlineKeyboardButton>>();
            foreach (var anime in model.Animes)
            {
                buttons.Add(new List<InlineKeyboardButton>()
                {
                    InlineKeyboardButton.WithCallbackData(anime.TitleRu,SubscriptionDubbingByAnimeCommand.Create(anime.Id,callbackQueryDataService,backNavigationArgs.CurrCommandData).Result)
                });
            }

            if (model.CountAllAnime > MaxCountPageOnMessage)
            {
                var lineWithNumberOfPages = new List<InlineKeyboardButton>();

                var start = model.Pagination.NumberOfPage - MaxCountPageOnMessage / 2;
                if (start < 1)
                    start = 1;
                else if (start + MaxCountPageOnMessage > model.CountPages)
                    start = Math.Max(1, model.CountPages - MaxCountPageOnMessage + 1);

                var finish = Math.Min(model.CountPages + 1, start + MaxCountPageOnMessage);
                for (int numberOfPage = start; numberOfPage < finish; numberOfPage++)
                {
                    var pagination = new PaginationModel()
                    {
                        CountPerPage = model.Pagination.CountPerPage,
                        NumberOfPage = numberOfPage,
                    };
                    var nameButtonPage = numberOfPage == model.Pagination.NumberOfPage ? $"✅{numberOfPage}" : numberOfPage.ToString();

                    lineWithNumberOfPages.Add(InlineKeyboardButton.WithCallbackData(nameButtonPage, UserSubscriptionsListCommand.Create(pagination, callbackQueryDataService).Result));
                }

                buttons.Add(lineWithNumberOfPages);
            }

            textMessage.ReplyMarkup = new InlineKeyboardMarkup(buttons);

            Messages.Add(mediaGroupMessage);
            Messages.Add(textMessage);
        }
    }
}
