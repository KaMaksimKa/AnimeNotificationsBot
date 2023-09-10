using AnimeNotificationsBot.Api.Enums;
using AnimeNotificationsBot.Api.Services.Commands.Base;
using AnimeNotificationsBot.Api.Services.Commands.Base.Args;
using AnimeNotificationsBot.Api.Services.Interfaces;
using AnimeNotificationsBot.Api.Services.Messages.Animes;
using AnimeNotificationsBot.Api.Services.Messages.Base;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.BLL.Models.Animes;

namespace AnimeNotificationsBot.Api.Services.Commands.TelegramCommands.Animes
{
    public class AnimeListCommand : CallbackCommand<AnimeListCommand, AnimeArgs>
    {
        private readonly IAnimeService _animeService;
        private readonly IBotSender _botSender;

        public AnimeListCommand(CallbackCommandArgs commandArgs, IAnimeService animeService, IBotSender botSender,
            ICallbackQueryDataService callbackQueryDataService) : base(commandArgs,callbackQueryDataService)
        {
            _animeService = animeService;
            _botSender = botSender;
        }

        public override CommandTypeEnum Type => CommandTypeEnum.Command;

        public override async Task ExecuteCommandAsync()
        {
            var data = await GetDataAsync();
            var animeArgs = data.Data;
            var animeListModel = await _animeService.GetAnimeWithImageByArgsAsync(animeArgs);
            await _botSender.ReplaceMessageAsync(new AnimeListMessage(animeListModel,CallbackQueryDataService,await GetBackNavigationArgs()),
                MessageId, ChatId,
                CancellationToken);
            await _botSender.AnswerCallbackQueryAsync(CommandArgs.CallbackQuery.Id,
                cancellationToken: CancellationToken);
        }
    }
}
