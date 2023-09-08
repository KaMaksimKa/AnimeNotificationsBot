using AnimeNotificationsBot.Api.Enums;
using AnimeNotificationsBot.Api.Services.Commands.Base;
using AnimeNotificationsBot.Api.Services.Commands.Base.Args;
using AnimeNotificationsBot.Api.Services.Interfaces;
using AnimeNotificationsBot.Api.Services.Messages.Anime;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.Common.Enums;

namespace AnimeNotificationsBot.Api.Services.Commands.TelegramCommands.Anime
{
    public class FindAnimeCommand : MessageCommand
    {
        private readonly IUserService _userService;
        private readonly IBotSender _botSender;
        private const string Name = "/find_anime";
        private const string FriendlyName = "Найти аниме";

        public FindAnimeCommand(MessageCommandArgs commandArgs, IUserService userService, IBotSender botSender) : base(commandArgs)
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
            await _userService.SetCommandStateAsync(TelegramUserId, CommandStateEnum.FindAnime);
            await _botSender.SendMessageAsync(new FindAnimeMessage(), ChatId, CommandArgs.CancellationToken);
        }

        public static string Create() => Name;

        public static string CreateFriendly() => FriendlyName;
    }
}
