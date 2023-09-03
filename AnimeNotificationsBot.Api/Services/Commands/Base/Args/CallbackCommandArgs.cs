using Telegram.Bot.Types;

namespace AnimeNotificationsBot.Api.Services.Commands.Base.Args
{
    public class CallbackCommandArgs: CommandArgs
    {
        public required CallbackQuery CallbackQuery { get; set; }
        public override long? TelegramUserId => CallbackQuery.From.Id;
        public override int? MessageId => CallbackQuery.Message?.MessageId;
        public override long? ChatId => CallbackQuery.Message?.Chat.Id;
        public override CancellationToken CancellationToken { get; set; }
    }
}
