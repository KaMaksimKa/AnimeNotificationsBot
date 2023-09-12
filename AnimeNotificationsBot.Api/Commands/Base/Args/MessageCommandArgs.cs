using Telegram.Bot.Types;

namespace AnimeNotificationsBot.Api.Commands.Base.Args
{
    public class MessageCommandArgs : CommandArgs
    {
        public required Message Message { get; set; }
        public override long? TelegramUserId => Message.From?.Id;
        public override int? MessageId => Message.MessageId;
        public override long? ChatId => Message.Chat.Id;
        public override CancellationToken CancellationToken { get; set; }
    }
}
