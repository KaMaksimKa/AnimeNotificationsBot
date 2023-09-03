using AnimeNotificationsBot.BLL.Models.BotMessage;

namespace AnimeNotificationsBot.BLL.Models.BotMessageGroup
{
    public class BotMessageGroupModel
    {
        public long ChatId { get; set; }
        public List<int> MessageIds { get; set; } = new List<int>();
    }
}
