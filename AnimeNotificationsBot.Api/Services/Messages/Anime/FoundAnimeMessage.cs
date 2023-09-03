using AnimeNotificationsBot.Api.Services.Commands.TelegramCommands.Anime;
using AnimeNotificationsBot.Api.Services.Messages.Base;
using AnimeNotificationsBot.BLL.Models.Anime;
using Telegram.Bot.Types.ReplyMarkups;

namespace AnimeNotificationsBot.Api.Services.Messages.Anime
{
    public class FoundAnimeMessage: TextMessage
    {
        public FoundAnimeMessage(IEnumerable<AnimeModel> animes, bool isAll)
        {
            if (isAll)
            {
                Text = $"""
                    Сортировка по точности совпадения.
                    Найдено по вашему запросу:
                    """ ;
            }
            else
            {
                Text = $"""
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
                    InlineKeyboardButton.WithCallbackData(anime.TitleRu,AnimeInfoCommand.Create(anime.Id))
                });
            }

            ReplyMarkup = new InlineKeyboardMarkup(buttons);
        }
    }
}
