using AnimeNotificationsBot.Api.Commands.Base;
using AnimeNotificationsBot.Api.Commands.Base.Args;
using AnimeNotificationsBot.Api.Enums;
using AnimeNotificationsBot.Api.Services.Interfaces;
using AnimeNotificationsBot.Api.Services.Messages;

namespace AnimeNotificationsBot.Api.Commands.TelegramCommands
{
    public class HelpCommand : MessageCommand
    {
        private readonly IBotSender _botSender;
        private const string Name = "/help";
        private const string FriendlyName = "Помощь";

        public HelpCommand(MessageCommandArgs commandArgs, IBotSender botSender) : base(commandArgs)
        {
            _botSender = botSender;
        }

        public override CommandTypeEnum Type => CommandTypeEnum.Command;

        protected override bool CanExecuteCommand()
        {
            return CommandArgs.Message.Text == Name || CommandArgs.Message.Text == FriendlyName;
        }

        public override async Task ExecuteCommandAsync()
        {
            await _botSender.SendMessageAsync(new HelpMessage(), CommandArgs.Message.Chat.Id, CommandArgs.CancellationToken);
        }

        public static string CreateFriendly()
        {
            return $"{FriendlyName}";
        }
    }
}
