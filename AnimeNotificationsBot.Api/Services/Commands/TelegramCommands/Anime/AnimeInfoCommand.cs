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
    public class AnimeInfoCommand : CallbackCommand
    {
        private readonly IAnimeService _animeService;
        private readonly IBotSender _botSender;
        private const string Name = "/anime_info";

        public AnimeInfoCommand(CallbackCommandArgs commandArgs, IAnimeService animeService, IBotSender botSender,
            ICallbackQueryDataService callbackQueryDataService) : base(commandArgs,callbackQueryDataService)
        {
            _animeService = animeService;
            _botSender = botSender;
        }

        public override CommandTypeEnum Type => CommandTypeEnum.TextCommand;

        protected override bool CanExecuteCommand()
        {
            return GetCommand() == Name;
        }

        public override async Task ExecuteCommandAsync()
        {
            var animeId = await GetDataAsync<long>();

            var anime = await _animeService.GetAnimeWithImageAsync(animeId);

            await _botSender.ReplaceMessageAsync(new AnimeInfoMessage(anime,new BackNavigationArgs()
            {
                ChildrenBackData = GetBackCommand()
            }),MessageId, ChatId,CancellationToken);
            await _botSender.AnswerCallbackQueryAsync(CommandArgs.CallbackQuery.Id, cancellationToken: CancellationToken);
        }


        public static async Task<string> Create(long animeId,ICallbackQueryDataService callbackQueryDataService,string? backCommand = null)
        {
            return await Create(Name,animeId, callbackQueryDataService, backCommand);
        }
    }
}
