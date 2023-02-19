namespace AnimeNotificationsBot.Api.Services.Commands.Base
{
    public abstract class CallbackCommand:ITelegramCommand
    {
        protected CallbackCommandArgs CommandArgs;

        protected CallbackCommand(CallbackCommandArgs commandArgs)
        {
            CommandArgs = commandArgs;
        }
        public abstract bool CanExecute();
        public abstract Task ExecuteAsync();
    }
}
