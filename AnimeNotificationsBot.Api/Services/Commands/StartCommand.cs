using AnimeNotificationsBot.Api.Services.Commands.Base;

namespace AnimeNotificationsBot.Api.Services.Commands
{
    public class StartCommand:MessageCommand
    {
        private const string CommandName = "/start";
        public StartCommand(MessageCommandArgs commandArgs) : base(commandArgs)
        {
        }

        public override bool CanExecute()
        {
            return CommandArgs.Message.Text == CommandName;
        }

        public override async Task ExecuteAsync()
        {
            if (!CanExecute())
            {
                throw new ArgumentException();
            }
        }
    }
}
