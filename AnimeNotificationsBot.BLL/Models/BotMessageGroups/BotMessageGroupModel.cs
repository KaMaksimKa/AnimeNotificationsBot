namespace AnimeNotificationsBot.BLL.Models.BotMessageGroups
{
    public class BotMessageGroupModel
    {
        public long ChatId { get; set; }
        public List<int> MessageIds { get; set; } = new List<int>();
    }
}
