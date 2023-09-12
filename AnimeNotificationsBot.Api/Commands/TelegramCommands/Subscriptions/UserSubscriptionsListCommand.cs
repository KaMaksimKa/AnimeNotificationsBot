using AnimeNotificationsBot.Api.Commands.Base;
using AnimeNotificationsBot.Api.Commands.Base.Args;
using AnimeNotificationsBot.Api.Enums;
using AnimeNotificationsBot.Api.Services.Interfaces;
using AnimeNotificationsBot.Api.Services.Messages.Subscriptions;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.BLL.Models;

namespace AnimeNotificationsBot.Api.Commands.TelegramCommands.Subscriptions
{
    public class UserSubscriptionsListCommand : CallbackCommand<UserSubscriptionsListCommand, PaginationModel>
    {
        private readonly IAnimeSubscriptionService _animeSubscriptionService;
        private readonly IBotSender _botSender;

        public UserSubscriptionsListCommand(CallbackCommandArgs commandArgs, ICallbackQueryDataService callbackQueryDataService,
            IAnimeSubscriptionService animeSubscriptionService, IBotSender botSender) : base(commandArgs, callbackQueryDataService)
        {
            _animeSubscriptionService = animeSubscriptionService;
            _botSender = botSender;
        }

        public override CommandTypeEnum Type => CommandTypeEnum.Command;

        public override async Task ExecuteCommandAsync()
        {
            var data = await GetDataAsync();
            var model = await _animeSubscriptionService.GetUserSubscriptionsListModelAsync(data.Data, TelegramUserId);
            await _botSender.ReplaceMessageAsync(
                new UserSubscriptionsListMessage(model, await GetBackNavigationArgs(), CallbackQueryDataService),
                MessageId, ChatId, CancellationToken);
        }
    }
}
