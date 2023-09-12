using AnimeNotificationsBot.Api.Commands.Base;
using AnimeNotificationsBot.Api.Commands.Base.Args;
using AnimeNotificationsBot.Api.Enums;
using AnimeNotificationsBot.Api.Models;
using AnimeNotificationsBot.Api.Services.Interfaces;
using AnimeNotificationsBot.Api.Services.Messages.Base;
using AnimeNotificationsBot.Api.Services.Messages.Subscriptions;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.BLL.Models.Subscriptions;

namespace AnimeNotificationsBot.Api.Commands.TelegramCommands.Subscriptions
{
    public class SubscriptionDubbingByAnimeCommand : CallbackCommand<SubscriptionDubbingByAnimeCommand, long>
    {
        private readonly IAnimeService _animeService;
        private readonly IAnimeSubscriptionService _subscriptionService;
        private readonly IBotSender _botSender;

        public SubscriptionDubbingByAnimeCommand(CallbackCommandArgs commandArgs, ICallbackQueryDataService callbackQueryDataService,
            IAnimeService animeService, IAnimeSubscriptionService subscriptionService, IBotSender botSender) : base(commandArgs, callbackQueryDataService)
        {
            _animeService = animeService;
            _subscriptionService = subscriptionService;
            _botSender = botSender;
        }

        public override CommandTypeEnum Type => CommandTypeEnum.Command;

        public override async Task ExecuteCommandAsync()
        {
            var data = await GetDataAsync();
            var animeId = data.Data;

            var anime = await _animeService.GetAnimeWithImageAsync(animeId);
            var subDubbing = await _subscriptionService.GetUserSubscriptionsByAnimeAsync(animeId, TelegramUserId);

            await _botSender.ReplaceMessageAsync(new ListDubbingForSubMessage(anime, subDubbing, new SubscribeAnimeModel()
            {
                AnimeId = animeId
            }, await GetBackNavigationArgs(), CallbackQueryDataService), MessageId, ChatId, CancellationToken);

            await _botSender.AnswerCallbackQueryAsync(CommandArgs.CallbackQuery.Id, cancellationToken: CancellationToken);
        }

    }
}
