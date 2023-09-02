using AnimeNotificationsBot.Api.Services.Commands.Base;
using AnimeNotificationsBot.Api.Services.Interfaces;
using AnimeNotificationsBot.Api.Services.Messages;

namespace AnimeNotificationsBot.Api.Services.Commands
{
    public class MainCallbackQueryCommand: CallbackCommand
    {
        private readonly ITelegramCommand _child;
        private readonly IBotSender _botSender;

        public MainCallbackQueryCommand(CallbackCommandArgs commandArgs, ITelegramCommand child, IBotSender botSender) : base(commandArgs)
        {
            _child = child;
            _botSender = botSender;
        }

        public override bool CanExecute() => true;
        public override async Task ExecuteAsync()
        {
            if (_child.CanExecute())
            {
                try
                {
                    await _child.ExecuteAsync();
                }
                catch (Exception e)
                {
                    await _botSender.SendMessageAsync(new SmthWentWrongMessage(e.Message),
                        CommandArgs.CallbackQuery.Message!.Chat.Id, cancellationToken: CommandArgs.CancellationToken);
                }
            }
            else
            {
                await _botSender.SendMessageAsync(new UndefinedCommandMessage(),
                    CommandArgs.CallbackQuery.Message!.Chat.Id, cancellationToken: CommandArgs.CancellationToken);
            }
        }
    }
}
