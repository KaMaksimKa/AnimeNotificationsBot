using AnimeNotificationsBot.Api.Commands.Base.Args;
using AnimeNotificationsBot.Api.Enums;

namespace AnimeNotificationsBot.Api.Commands.Base
{
    public abstract class TelegramCommand : ITelegramCommand
    {
        private readonly long? _telegramUserId;
        private readonly int? _messageId;
        private readonly long? _chatId;

        protected long TelegramUserId => _telegramUserId!.Value;
        protected int MessageId => _messageId!.Value;
        protected long ChatId => _chatId!.Value;
        protected CancellationToken CancellationToken { get; }

        public abstract CommandTypeEnum Type { get; }

        protected TelegramCommand(CommandArgs commandArgs)
        {
            _telegramUserId = commandArgs.TelegramUserId;
            _messageId = commandArgs.MessageId;
            _chatId = commandArgs.ChatId;
            CancellationToken = commandArgs.CancellationToken;
        }

        public virtual bool CanExecute()
        {
            return _telegramUserId.HasValue && _messageId.HasValue && _chatId.HasValue;
        }

        public virtual Task ExecuteAsync()
        {
            if (!CanExecute())
                throw new ArgumentException(GetType().Name);

            return Task.CompletedTask;
        }
    }
}
