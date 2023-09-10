using AnimeNotificationsBot.Api.Services.Commands.Base.Args;
using AnimeNotificationsBot.Api.Services.Messages.Base;

namespace AnimeNotificationsBot.Api.Services.Commands.Base
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
                throw new ArgumentException(this.GetType().Name);

            await ExecuteCommandAsync();
        }

        public abstract Task ExecuteCommandAsync();

    }
}
