using AnimeNotificationsBot.Api.Services.Commands.Anime;
using AnimeNotificationsBot.Api.Services.Messages.Base;
using AnimeNotificationsBot.BLL.Models.Anime;
using Telegram.Bot.Types.ReplyMarkups;

namespace AnimeNotificationsBot.Api.Services.Messages.Anime
{
    public class FoundAnimeMessage: TextMessage
    {
        public FoundAnimeMessage(IEnumerable<AnimeModel> animes)
        {
            Text = $"""
                Да, да, да аааааааааааааааааааааааааааааааааааааа
                """;//todo

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
