using AnimeNotificationsBot.Api.Services.Commands.Base;
using AnimeNotificationsBot.Api.Services.Interfaces;
using AnimeNotificationsBot.Api.Services.Messages;
using AnimeNotificationsBot.Api.Services.Messages.Base;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.BLL.NewFolder.NewFolder;
using AnimeNotificationsBot.DAL;

namespace AnimeNotificationsBot.Api.Services.Commands
{
    public class StartCommand:MessageCommand
    {
        private readonly IUserService _userService;
        private readonly IBotSender _botSender;
        private const string _name = "/start";

        public StartCommand(MessageCommandArgs commandArgs, IUserService userService,
             IBotSender botSender) : base(commandArgs)
        {
            _userService = userService;
            _botSender = botSender;
        }

        public override bool CanExecute()
        {
            if (CommandArgs.Message.From == null)
                return false;
            
            return CommandArgs.Message.Text == _name;
        }

        public override async Task ExecuteAsync()
        {
            if (!CanExecute())
            {
                throw new ArgumentException();
            }

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

            await _botSender.SendMessageAsync(new MenuMessage(),
                message.Chat.Id, cancellationToken: CommandArgs.CancellationToken);
        }
        public static string Create()
        {
            return $"{_name}";
        }
    }
}
