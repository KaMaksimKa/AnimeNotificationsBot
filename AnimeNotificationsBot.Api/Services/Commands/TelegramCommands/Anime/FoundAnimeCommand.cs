using AnimeNotificationsBot.Api.Enums;
using AnimeNotificationsBot.Api.Models;
using AnimeNotificationsBot.Api.Services.Commands.Base;
using AnimeNotificationsBot.Api.Services.Commands.Base.Args;
using AnimeNotificationsBot.Api.Services.Interfaces;
using AnimeNotificationsBot.Api.Services.Messages.Anime;
using AnimeNotificationsBot.Api.Services.Messages.Base;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.BLL.Models.Animes;
using AnimeNotificationsBot.Common.Enums;

namespace AnimeNotificationsBot.Api.Services.Commands.TelegramCommands.Anime
{
    public class FoundAnimeCommand : MessageCommand
    {
        private const int MaxFoundAnimeCount = 10;

        private readonly IAnimeService _animeService;
        private readonly IUserService _userService;
        private readonly IBotSender _botSender;
        private readonly ICallbackQueryDataService _callbackQueryDataService;

        public FoundAnimeCommand(MessageCommandArgs commandArgs, IAnimeService animeService, IUserService userService,
            IBotSender botSender,ICallbackQueryDataService callbackQueryDataService) : base(commandArgs)
        {
            _animeService = animeService;
            _userService = userService;
            _botSender = botSender;
            _callbackQueryDataService = callbackQueryDataService;
        }

        public override CommandTypeEnum Type => CommandTypeEnum.TextAnswer;

        protected override bool CanExecuteCommand()
        {
            return _userService.GetCommandStateAsync(TelegramUserId).Result == CommandStateEnum.FindAnime;
        }

        public override async Task ExecuteCommandAsync()
        {
            var animeListModel = await _animeService.GetAnimeWithImageByArgsAsync(new AnimeArgs()
            {
                SearchQuery = CommandArgs.Message.Text
            });

            await _userService.SetCommandStateAsync(TelegramUserId, CommandStateEnum.None);

            if (animeListModel.Animes.Count != 0)
            {
                await _botSender.SendMessageAsync(new AnimeListMessage(animeListModel, _callbackQueryDataService, new BackNavigationArgs()
                {
                    CurrCommandData = await AnimeListCommand.Create(new AnimeArgs()
                    {
                        SearchQuery = CommandArgs.Message.Text
                    }, _callbackQueryDataService)
                }), ChatId, CommandArgs.CancellationToken);
            }
            else
            {
                await _botSender.SendMessageAsync(new NotFoundAnimeMessage(), ChatId, CancellationToken);
            }

            

            
        }
    }
}
