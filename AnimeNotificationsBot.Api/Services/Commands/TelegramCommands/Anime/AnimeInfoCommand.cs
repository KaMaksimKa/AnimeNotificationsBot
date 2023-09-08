using AnimeNotificationsBot.Api.Enums;
using AnimeNotificationsBot.Api.Services.Commands.Base;
using AnimeNotificationsBot.Api.Services.Commands.Base.Args;
using AnimeNotificationsBot.Api.Services.Interfaces;
using AnimeNotificationsBot.Api.Services.Messages.Anime;
using AnimeNotificationsBot.Api.Services.Messages.Base;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.Common.Enums;

namespace AnimeNotificationsBot.Api.Services.Commands.TelegramCommands.Anime
{
    public class AnimeInfoCommand : CallbackCommand<AnimeInfoCommand,long>
    {
        private readonly IAnimeService _animeService;
        private readonly IBotSender _botSender;

        public AnimeInfoCommand(CallbackCommandArgs commandArgs, IAnimeService animeService, IBotSender botSender,
            ICallbackQueryDataService callbackQueryDataService) : base(commandArgs,callbackQueryDataService)
        {
            _animeService = animeService;
            _botSender = botSender;
        }

        public override CommandTypeEnum Type => CommandTypeEnum.Command;

        protected override bool CanExecuteCommand()
        {
            return GetCommandNameFromQuery() == Name;
        }

        public override async Task ExecuteCommandAsync()
        {
            var data = await GetDataAsync();
            var animeId = data.Data;

            var model = await _animeService.GetAnimeInfoModel(animeId);

            await _botSender.ReplaceMessageAsync(new AnimeInfoMessage(model, new BackNavigationArgs()
            {
                PrevCommandData = data.PrevStringCommand,
                CurrCommandData = GetCurrCommandFromQuery()
            },CallbackQueryDataService),MessageId, ChatId,CancellationToken);
            await _botSender.AnswerCallbackQueryAsync(CommandArgs.CallbackQuery.Id, cancellationToken: CancellationToken);
        }
    }
}
