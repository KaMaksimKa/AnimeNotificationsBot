using AnimeNotificationsBot.Api.Commands.TelegramCommands.Subscriptions;
using AnimeNotificationsBot.Api.Messages.Animes;
using AnimeNotificationsBot.Api.Messages.Base;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.BLL.Models;
using AnimeNotificationsBot.BLL.Models.Subscriptions;
using Telegram.Bot.Types.ReplyMarkups;

namespace AnimeNotificationsBot.Api.Messages.Subscriptions
{
    public class UserSubscriptionsListMessage : CombiningMessage
    {
        private const int MaxCountPageOnMessage = 5;
        public UserSubscriptionsListMessage(UserSubscriptionListModel model, BackNavigationArgs backNavigationArgs,
            ICallbackQueryDataService callbackQueryDataService)
        {
            if (model.Animes.Any())
            {
                TextMessage textMessage;

                if (model.Animes.Count == 1)
                {
                    textMessage = new PhotoMessage()
                    {
                        ImgHref = model.Animes.First().ImgHref,
                    };
                }
                else
                {
                    var animeImagesMessage = new AnimeImagesMessage(model.Animes);
                    Messages.Add(animeImagesMessage);

                    textMessage = new TextMessage();

                }

                textMessage.Text = $"Список тайтлов на которые ты подписан: <i>(стр. {model.Pagination.NumberOfPage} из {model.CountPages})</i>";
                textMessage.ParseMode = Telegram.Bot.Types.Enums.ParseMode.Html;

                var buttons = new List<List<InlineKeyboardButton>>();
                foreach (var anime in model.Animes)
                {
                    buttons.Add(new List<InlineKeyboardButton>()
                    {
                        InlineKeyboardButton.WithCallbackData(anime.TitleRu,
                            SubscriptionDubbingByAnimeCommand.Create(anime.Id, callbackQueryDataService,
                                backNavigationArgs.CurrCommandData).Result)
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
                        var pagination = model.Pagination.Copy();
                        pagination.NumberOfPage = numberOfPage;

                        var nameButtonPage = numberOfPage == model.Pagination.NumberOfPage
                            ? $"✅{numberOfPage}"
                            : numberOfPage.ToString();

                        lineWithNumberOfPages.Add(InlineKeyboardButton.WithCallbackData(nameButtonPage,
                            UserSubscriptionsListCommand.Create(pagination, callbackQueryDataService).Result));
                    }

                    buttons.Add(lineWithNumberOfPages);
                }

                textMessage.ReplyMarkup = new InlineKeyboardMarkup(buttons);

                Messages.Add(textMessage);
            }
            else
            {
                Messages.Add(new TextMessage()
                {
                    Text = "Ты еще не подписан ни на одно аниме"
                });
            }
        }
    }
}
