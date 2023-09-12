using AnimeNotificationsBot.Api.Commands.Base;
using AnimeNotificationsBot.Api.Commands.Base.Args;
using AnimeNotificationsBot.Api.Enums;
using AnimeNotificationsBot.Api.Messages.Animes;
using AnimeNotificationsBot.Api.Services.Interfaces;
using AnimeNotificationsBot.BLL.Interfaces;

namespace AnimeNotificationsBot.Api.Commands.TelegramCommands.Animes
{
    public class AnimeInfoCommand : CallbackCommand<AnimeInfoCommand, long>
    {
        private readonly IAnimeService _animeService;
        private readonly IBotSender _botSender;

        public AnimeInfoCommand(CallbackCommandArgs commandArgs, IAnimeService animeService, IBotSender botSender,
            ICallbackQueryDataService callbackQueryDataService) : base(commandArgs, callbackQueryDataService)
        {
            _animeService = animeService;
            _botSender = botSender;
        }

        public override CommandTypeEnum Type => CommandTypeEnum.Command;

        public override async Task ExecuteCommandAsync()
        {
            var data = await GetDataAsync();
            var animeId = data.Data;

            var model = await _animeService.GetAnimeInfoModel(animeId);

            await _botSender.ReplaceMessageAsync(new AnimeInfoMessage(model, await GetBackNavigationArgs(), CallbackQueryDataService), MessageId, ChatId, CancellationToken);
            await _botSender.AnswerCallbackQueryAsync(CommandArgs.CallbackQuery.Id, cancellationToken: CancellationToken);
        }
    }
}
