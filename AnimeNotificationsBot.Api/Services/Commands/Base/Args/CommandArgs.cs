namespace AnimeNotificationsBot.Api.Services.Commands.Base.Args
{
    public abstract class CommandArgs
    {
        public abstract long? TelegramUserId { get; }
        public abstract int? MessageId { get; }
        public abstract long? ChatId { get; }
        public abstract CancellationToken CancellationToken { get; set; }
    }
}
