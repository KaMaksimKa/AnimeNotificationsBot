using AnimeNotificationsBot.Api.Enums;
using AnimeNotificationsBot.Api.Services.Commands.Base;
using AnimeNotificationsBot.Api.Services.Commands.Base.Args;
using AnimeNotificationsBot.Api.Services.Interfaces;
using AnimeNotificationsBot.Api.Services.Messages;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.BLL.Models.Users;

namespace AnimeNotificationsBot.Api.Services.Commands.TelegramCommands
{
    public class StartCommand : MessageCommand
    {
        private readonly IUserService _userService;
        private readonly IBotSender _botSender;
        private const string Name = "/start";

        public StartCommand(MessageCommandArgs commandArgs, IUserService userService,
             IBotSender botSender) : base(commandArgs)
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
            var message = CommandArgs.Message;
            var telegramUser = message.From!;

            await _userService.CreateUserAsync(new CreateUserModel()
            {
                FirstName = telegramUser.FirstName,
                LastName = telegramUser.LastName,
                TelegramChatId = message.Chat.Id,
                TelegramUserId = telegramUser.Id,
                UserName = telegramUser.Username
            });

            await _botSender.SendMessageAsync(new MenuMessage(), ChatId, cancellationToken: CancellationToken);
        }

        public static string Create() => Name;
    }
}
