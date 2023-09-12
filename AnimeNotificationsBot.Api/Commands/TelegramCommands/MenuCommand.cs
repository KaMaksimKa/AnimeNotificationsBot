using AnimeNotificationsBot.Api.Commands.Base;
using AnimeNotificationsBot.Api.Commands.Base.Args;
using AnimeNotificationsBot.Api.Enums;
using AnimeNotificationsBot.Api.Messages;
using AnimeNotificationsBot.Api.Services.Interfaces;

namespace AnimeNotificationsBot.Api.Commands.TelegramCommands
{
    public class MenuCommand : MessageCommand
    {
        private readonly IBotSender _botSender;
        private const string Name = "/menu";

        public MenuCommand(MessageCommandArgs commandArgs, IBotSender botSender) : base(commandArgs)
        {
            _botSender = botSender;
        }

        public override CommandTypeEnum Type => CommandTypeEnum.Command;

        protected override bool CanExecuteCommand()
        {
            return CommandArgs.Message.Text == Name;
        }

        public override async Task ExecuteCommandAsync()
        {
            await _botSender.SendMessageAsync(new MenuMessage(), ChatId, cancellationToken: CancellationToken);
        }

        public static string Create() => Name;
    }
}
