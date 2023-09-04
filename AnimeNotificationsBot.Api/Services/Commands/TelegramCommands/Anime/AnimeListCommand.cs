using AnimeNotificationsBot.Api.Enums;
using AnimeNotificationsBot.Api.Services.Commands.Base;
using AnimeNotificationsBot.Api.Services.Commands.Base.Args;
using AnimeNotificationsBot.Api.Services.Interfaces;
using AnimeNotificationsBot.Api.Services.Messages.Anime;
using AnimeNotificationsBot.Api.Services.Messages.Base;
using AnimeNotificationsBot.BLL.Enums;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.BLL.Models.Anime;


namespace AnimeNotificationsBot.Api.Services.Commands.TelegramCommands.Anime
{
    public class AnimeListCommand : CallbackCommand
    {
        private readonly IAnimeService _animeService;
        private readonly IBotSender _botSender;
        private const string Name = "anime_list";

        public AnimeListCommand(CallbackCommandArgs commandArgs, IAnimeService animeService, IBotSender botSender,
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
            var animeArgs = await GetDataAsync<AnimeArgs>();
            var animeListModel = await _animeService.GetAnimeWithImageByArgsAsync(animeArgs);
            await _botSender.ReplaceMessageAsync(new AnimeListMessage(animeListModel,CallbackQueryDataService,new BackNavigationArgs()
                {
                    ChildrenBackData = await Create(animeArgs,CallbackQueryDataService)
                }), MessageId, ChatId,
                CancellationToken);
            await _botSender.AnswerCallbackQueryAsync(CommandArgs.CallbackQuery.Id,
                cancellationToken: CancellationToken);
        }

        public static async Task<string> Create(AnimeArgs animeArgs, ICallbackQueryDataService callbackQueryDataService)
        {
            return await Create(Name, animeArgs,callbackQueryDataService);
        }


    }
}
