using AnimeNotificationsBot.Api.Commands.Base;
using AnimeNotificationsBot.Api.Commands.Base.Args;
using AnimeNotificationsBot.Api.Enums;
using AnimeNotificationsBot.Api.Services.Interfaces;
using AnimeNotificationsBot.Api.Services.Messages.Feedbacks;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.Common.Enums;

namespace AnimeNotificationsBot.Api.Commands.TelegramCommands.Feedbacks
{
    public class SentFeedbackCommand : MessageCommand
    {
        private readonly IFeedbackService _feedbackService;
        private readonly IUserService _userService;
        private readonly IBotSender _botSender;

        public SentFeedbackCommand(MessageCommandArgs commandArgs, IFeedbackService feedbackService,
            IUserService userService, IBotSender botSender) : base(commandArgs)
        {
            _feedbackService = feedbackService;
            _userService = userService;
            _botSender = botSender;
        }

        public override CommandTypeEnum Type => CommandTypeEnum.TextAnswer;
        protected override bool CanExecuteCommand()
        {
            return _userService.GetCommandStateAsync(TelegramUserId).Result == CommandStateEnum.SendFeedback;
        }

        public override async Task ExecuteCommandAsync()
        {
            await _feedbackService.AddAsync(CommandArgs.Message.Text ?? "", TelegramUserId);
            await _userService.SetCommandStateAsync(TelegramUserId, CommandStateEnum.None);
            await _botSender.SendMessageAsync(new SentFeedbackMessage(), ChatId, CancellationToken);
        }
    }
}
