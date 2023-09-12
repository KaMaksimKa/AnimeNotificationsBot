using AnimeNotificationsBot.Api.Commands.Base;
using AnimeNotificationsBot.Api.Commands.Base.Args;
using AnimeNotificationsBot.Api.Enums;
using AnimeNotificationsBot.Api.Messages.Animes;
using AnimeNotificationsBot.Api.Messages.Base;
using AnimeNotificationsBot.Api.Services.Interfaces;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.BLL.Models.Animes;

namespace AnimeNotificationsBot.Api.Commands.TelegramCommands.Animes
{
    public class OngoingAnimeCommand:MessageCommand
    {
        private readonly IAnimeService _animeService;
        private readonly IBotSender _botSender;
        private readonly ICallbackQueryDataService _callbackQueryDataService;
        private const string Name = "/ongoings";
        private const string FriendlyName = "🚀Онгоинги";
        public OngoingAnimeCommand(MessageCommandArgs commandArgs,IAnimeService animeService, IBotSender botSender,
            ICallbackQueryDataService callbackQueryDataService) : base(commandArgs)
        {
            _animeService = animeService;
            _botSender = botSender;
            _callbackQueryDataService = callbackQueryDataService;
        }

        public override CommandTypeEnum Type => CommandTypeEnum.Command;
        protected override bool CanExecuteCommand()
        {
            return CommandArgs.Message.Text == FriendlyName || CommandArgs.Message.Text == Name;
        }

        public override async Task ExecuteCommandAsync()
        {
            var args = new AnimeArgs()
            {
                OnlyOngoing = true
            };
            var model = await _animeService.GetAnimeWithImageByArgsAsync(args);

            await _botSender.SendMessageAsync(new AnimeListMessage(model, _callbackQueryDataService,
                new BackNavigationArgs()
                {
                    CurrCommandData = await AnimeListCommand.Create(args, _callbackQueryDataService)
                }),ChatId,CancellationToken);
        }

        public static string CreateFriendly() => FriendlyName;
    }
}
