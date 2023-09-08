using AnimeNotificationsBot.Api.Models;
using AnimeNotificationsBot.Api.Services.Commands.TelegramCommands.Subscriptions;
using AnimeNotificationsBot.Api.Services.Messages.Base;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.BLL.Models.Anime;
using AnimeNotificationsBot.BLL.Models.Subscription;
using Telegram.Bot.Types.ReplyMarkups;

namespace AnimeNotificationsBot.Api.Services.Messages.Subscription
{
    public class ListDubbingForSubMessage:PhotoMessage
    {
        public ListDubbingForSubMessage(AnimeWithImageModel anime,List<SubscriptionDubbingModel> dubbing, SubscribeAnimeModel model,
            BackNavigationArgs backNavigationArgs, ICallbackQueryDataService callbackQueryDataService)
        {
            Photo = anime.Image;

            Text = $"Информация об уведомления по аниме: {anime.TitleRu}";

            var buttons = new List<List<InlineKeyboardButton>>();

            foreach (var sub in dubbing)
            {
                var subModel = new SubscribeAnimeModel()
                {
                    AnimeId = model.AnimeId,
                    DubbingId = sub.Dubbing.Id,
                };

                buttons.Add(new List<InlineKeyboardButton>()
                {
                    InlineKeyboardButton.WithCallbackData($"{(sub.HasSubscribe?"✅":"❌")} {sub.Dubbing.Title}",SubscribedAnimeCommand.Create(new SubcribeAnimeExtModel()
                    {
                        SubscribeAnimeModel = subModel,
                        Unsubscribe = sub.HasSubscribe
                    },callbackQueryDataService,backNavigationArgs.PrevCommandData).Result)
                });
            }


            if (backNavigationArgs.PrevCommandData != null)
            {
                buttons.Add(new List<InlineKeyboardButton>()
                {
                    InlineKeyboardButton.WithCallbackData("Назад",backNavigationArgs.PrevCommandData)
                });
            }
            

            ReplyMarkup = new InlineKeyboardMarkup(buttons);
        }
    }
}
