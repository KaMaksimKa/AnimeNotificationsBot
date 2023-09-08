using AnimeNotificationsBot.Api.Enums;
using AnimeNotificationsBot.Api.Services.Commands.Base;
using AnimeNotificationsBot.Api.Services.Commands.Base.Args;
using AnimeNotificationsBot.Api.Services.Interfaces;
using AnimeNotificationsBot.Api.Services.Messages;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.Common.Enums;

namespace AnimeNotificationsBot.Api.Services.Commands.TelegramCommands
{
    public class CancelCommand : MessageCommand
    {
        private readonly IUserService _userService;
        private readonly IBotSender _botSender;
        private const string Name = "/cancel";

        public CancelCommand(MessageCommandArgs commandArgs, IUserService userService, IBotSender botSender) : base(commandArgs)
        {
            _userService = userService;
            _botSender = botSender;
        }

        public override CommandTypeEnum Type => CommandTypeEnum.Command;

        protected override bool CanExecuteCommand()
        {
            return CommandArgs.Message.Text == Name;
        }

        public override async Task ExecuteCommandAsync()
        {
            var commandState = await _userService.GetCommandStateAsync(TelegramUserId);

            if (commandState == CommandStateEnum.None)
            {
                await _botSender.SendMessageAsync(new NotExecutingCommandMessage(), ChatId, CancellationToken);
            }
            else
            {
                await _userService.SetCommandStateAsync(TelegramUserId, CommandStateEnum.None);
                await _botSender.SendMessageAsync(new CommandCancelledMessage(), ChatId, CancellationToken);
            }
        }

        public static string Create()
        {
            return $"{Name}";
        }
    }
}
