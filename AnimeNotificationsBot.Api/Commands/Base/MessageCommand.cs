using AnimeNotificationsBot.Api.Commands.Base.Args;

namespace AnimeNotificationsBot.Api.Commands.Base
{
    public abstract class MessageCommand : TelegramCommand, IMessageCommand
    {
        protected MessageCommandArgs CommandArgs;
        protected MessageCommand(MessageCommandArgs commandArgs) : base(commandArgs)
        {
            CommandArgs = commandArgs;
        }

        public sealed override bool CanExecute()
        {
            return base.CanExecute() && CanExecuteCommand();
        }

        protected abstract bool CanExecuteCommand();

        public sealed override async Task ExecuteAsync()
        {
            if (!CanExecute())
                throw new ArgumentException(GetType().Name);

            await ExecuteCommandAsync();
        }

        public abstract Task ExecuteCommandAsync();

    }
}
