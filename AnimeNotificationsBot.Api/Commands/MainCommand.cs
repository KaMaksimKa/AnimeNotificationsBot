using AnimeNotificationsBot.Api.Commands.Base;
using AnimeNotificationsBot.Api.Commands.Base.Args;
using AnimeNotificationsBot.Api.Messages;
using AnimeNotificationsBot.Api.Services.Interfaces;

namespace AnimeNotificationsBot.Api.Commands
{
    public class MainCommand : ICommand
    {
        private readonly CommandArgs _commandArgs;
        private readonly ICommand _child;
        private readonly IBotSender _botSender;

        public MainCommand(CommandArgs commandArgs, ICommand child, IBotSender botSender)
        {
            _commandArgs = commandArgs;
            _child = child;
            _botSender = botSender;
        }

        public bool CanExecute() => true;
        public async Task ExecuteAsync()
        {
            if (_child.CanExecute())
            {
                try
                {
                    await _child.ExecuteAsync();
                }
                catch (Exception e)
                {
                    if (_commandArgs.ChatId.HasValue)
                        await _botSender.SendMessageAsync(new SmthWentWrongMessage(e.Message),
                            _commandArgs.ChatId.Value, cancellationToken: _commandArgs.CancellationToken);
                }
            }
            else
            {
                if (_commandArgs.ChatId.HasValue)
                    await _botSender.SendMessageAsync(new UndefinedCommandMessage(),
                        _commandArgs.ChatId.Value, cancellationToken: _commandArgs.CancellationToken);
            }
        }
    }
}
