namespace AnimeNotificationsBot.Api.Services.Commands.Base
{
    public abstract class MessageCommand:ITelegramCommand
    {
        protected MessageCommandArgs CommandArgs;
        protected MessageCommand(MessageCommandArgs commandArgs)
        {
            CommandArgs = commandArgs;
        }
        public abstract bool CanExecute();
        public abstract Task ExecuteAsync();
    }
}
