using AnimeNotificationsBot.Api.Messages.Base;

namespace AnimeNotificationsBot.Api.Messages
{
    public class HelpMessage : TextMessage
    {
        public HelpMessage()
        {
            Text = """ 
                Всем привет! 
                Это новый бот, который поможет тебе отслеживать выход новых серий в твоей любимой озвучке на сайте Animego.org, а также удобный интерфейс для поиска и просмотра информации об аниме.

                Основные команды: 

                /find_anime - Найти аниме
                /subscriptions - Мои подписки
                /menu - Меню с основными командами бота
                /help - Помощь в управлении ботом
                /feedback - Оставить отзыв разработчику
                """;
        }
    }
}
