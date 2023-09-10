using AnimeNotificationsBot.Api.Enums;
using AnimeNotificationsBot.Api.Services.Commands.Base;
using AnimeNotificationsBot.Api.Services.Commands.Base.Args;
using AnimeNotificationsBot.Api.Services.Interfaces;
using AnimeNotificationsBot.Api.Services.Messages.Feedbacks;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.Common.Enums;

namespace AnimeNotificationsBot.Api.Services.Commands.TelegramCommands.Feedbacks
{
    public class SendFeedbackCommand : MessageCommand
    {
        private readonly IUserService _userService;
        private readonly IBotSender _botSender;
        private const string Name = "/feedback";
        private const string FriendlyName = "Оставить отзыв";

        public SendFeedbackCommand(MessageCommandArgs commandArgs, IUserService userService, IBotSender botSender) : base(commandArgs)
        {
            _userService = userService;
            _botSender = botSender;
        }

        public override CommandTypeEnum Type => CommandTypeEnum.Command;

        protected override bool CanExecuteCommand()
        {
            return (CommandArgs.Message.Text == Name || CommandArgs.Message.Text == FriendlyName);
        }

        public override async Task ExecuteCommandAsync()
        {
            await _userService.SetCommandStateAsync(TelegramUserId, CommandStateEnum.SendFeedback);
            await _botSender.SendMessageAsync(new SendFeedbackMessage(), ChatId, CommandArgs.CancellationToken);
        }

        public static string CreateFriendly() => FriendlyName;
    }
}
