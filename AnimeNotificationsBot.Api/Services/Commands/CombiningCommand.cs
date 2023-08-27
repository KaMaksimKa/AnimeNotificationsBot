using AnimeNotificationsBot.Api.Services.Commands.Base;

namespace AnimeNotificationsBot.Api.Services.Commands
{
    public class CombiningCommand:ITelegramCommand
    {
        private readonly IEnumerable<ITelegramCommand> _commands;
        public CombiningCommand(IEnumerable<ITelegramCommand> commands)
        {
            _commands = commands;
        }

        public bool CanExecute()
        {
            return _commands.Any(c => c.CanExecute());
        }

        public async Task ExecuteAsync()
        {
            if (!CanExecute())
            {
                throw new ArgumentException();
            }

            foreach (var command in _commands)
            {
                await command.ExecuteAsync();
            }
        }
    }
}
