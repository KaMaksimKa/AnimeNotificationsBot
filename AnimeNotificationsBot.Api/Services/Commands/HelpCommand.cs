using AnimeNotificationsBot.Api.Services.Commands.Base;

namespace AnimeNotificationsBot.Api.Services.Commands
{
    public class HelpCommand:MessageCommand
    {
        private const string CommandName = "/help";
        public HelpCommand(MessageCommandArgs commandArgs) : base(commandArgs)
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
