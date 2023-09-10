using AnimeNotificationsBot.Api.Services.Commands.TelegramCommands.Animes;
using AnimeNotificationsBot.Api.Services.Messages.Base;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.BLL.Models.Animes;
using Telegram.Bot.Types.ReplyMarkups;

namespace AnimeNotificationsBot.Api.Services.Messages.Animes
{
    public class FoundAnimeMessage: CombiningMessage
    {
        public FoundAnimeMessage(List<AnimeModel> animes, bool isAll,ICallbackQueryDataService callbackQueryDataService)
        {
            var mediaGroupMessage = new AnimeImagesMessage(animes);

            var textMessage = new TextMessage();
            if (isAll)
            {
                textMessage.Text = $"""
                    Сортировка по точности совпадения.
                    Найдено по вашему запросу:
                    """ ;
            }
            else
            {
                textMessage.Text = $"""
                    Сортировка по точности совпадения.
                    Не все результаты влезли в этот список, попробуй уточнить запрос если не нашел нужного тайтла!
                    Найдено по вашему запросу:
                    """;
            }
            

            var buttons = new List<List<InlineKeyboardButton>>();
            foreach (var anime in animes)
            {
                buttons.Add(new List<InlineKeyboardButton>()
                {
                    InlineKeyboardButton.WithCallbackData(anime.TitleRu,AnimeInfoCommand.Create(anime.Id, callbackQueryDataService).Result)
                });
            }

            textMessage.ReplyMarkup = new InlineKeyboardMarkup(buttons);


            Messages.Add(mediaGroupMessage);
            Messages.Add(textMessage);
        }
    }
}
