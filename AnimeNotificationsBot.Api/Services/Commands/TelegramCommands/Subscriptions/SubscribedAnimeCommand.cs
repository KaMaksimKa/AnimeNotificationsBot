using AnimeNotificationsBot.Api.Enums;
using AnimeNotificationsBot.Api.Models;
using AnimeNotificationsBot.Api.Services.Commands.Base;
using AnimeNotificationsBot.Api.Services.Commands.Base.Args;
using AnimeNotificationsBot.Api.Services.Interfaces;
using AnimeNotificationsBot.Api.Services.Messages.Base;
using AnimeNotificationsBot.Api.Services.Messages.Subscription;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.BLL.Models.Subscription;

namespace AnimeNotificationsBot.Api.Services.Commands.TelegramCommands.Subscriptions
{
    public class SubscribedAnimeCommand : CallbackCommand<SubscribedAnimeCommand, SubcribeAnimeExtModel>
    {
        private readonly IAnimeSubscriptionService _subscriptionService;
        private readonly IBotSender _botSender;
        private readonly IAnimeService _animeService;

        public SubscribedAnimeCommand(CallbackCommandArgs commandArgs, ICallbackQueryDataService callbackQueryDataService,
            IAnimeSubscriptionService subscriptionService, IBotSender botSender, IAnimeService animeService) : base(commandArgs, callbackQueryDataService)
        {
            _subscriptionService = subscriptionService;
            _botSender = botSender;
            _animeService = animeService;
        }

        public override CommandTypeEnum Type => CommandTypeEnum.Command;
        protected override bool CanExecuteCommand()
        {
            return GetCommandNameFromQuery() == Name;
        }

        public override async Task ExecuteCommandAsync()
        {
            var data = await GetDataAsync();
            var subAnimeModel = data.Data;
            if (subAnimeModel.Unsubscribe)
                await _subscriptionService.UnsubscribeAsync(subAnimeModel.SubscribeAnimeModel, TelegramUserId);
            else
                await _subscriptionService.SubscribeAsync(subAnimeModel.SubscribeAnimeModel, TelegramUserId);

            var anime = await _animeService.GetAnimeWithImageAsync(subAnimeModel.SubscribeAnimeModel.AnimeId!.Value);
            var subDubbing = await _subscriptionService.GetSubscriptionsAsync(subAnimeModel.SubscribeAnimeModel.AnimeId.Value,
                TelegramUserId);

            await _botSender.AnswerCallbackQueryAsync(CommandArgs.CallbackQuery.Id, cancellationToken: CancellationToken);
            await _botSender.EditReplyMarkupAsync(new ListDubbingForSubMessage(anime, subDubbing, subAnimeModel.SubscribeAnimeModel,
                new BackNavigationArgs()
                {
                    PrevCommandData = data.PrevStringCommand
                },CallbackQueryDataService), MessageId, ChatId, CancellationToken);
        }

    }
}
