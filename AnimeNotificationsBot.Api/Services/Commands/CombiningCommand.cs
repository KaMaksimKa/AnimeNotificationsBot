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

            var commands = _commands.Where(x => x.CanExecute());

            foreach (var command in commands)
            {
                await command.ExecuteAsync();
            }
        }
    }
}
