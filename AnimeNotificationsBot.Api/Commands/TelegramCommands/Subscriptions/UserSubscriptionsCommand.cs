using AnimeNotificationsBot.Api.Commands.Base;
using AnimeNotificationsBot.Api.Commands.Base.Args;
using AnimeNotificationsBot.Api.Enums;
using AnimeNotificationsBot.Api.Messages.Base;
using AnimeNotificationsBot.Api.Messages.Subscriptions;
using AnimeNotificationsBot.Api.Services.Interfaces;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.BLL.Models;

namespace AnimeNotificationsBot.Api.Commands.TelegramCommands.Subscriptions
{
    public class UserSubscriptionsCommand : MessageCommand
    {
        private readonly IAnimeSubscriptionService _subscriptionService;
        private readonly IBotSender _botSender;
        private readonly ICallbackQueryDataService _callbackQueryDataService;
        private const string Name = "/subscriptions";
        private const string FriendlyName = "💕Мои Подписки";
        public UserSubscriptionsCommand(MessageCommandArgs commandArgs, IAnimeSubscriptionService subscriptionService,
            IBotSender botSender, ICallbackQueryDataService callbackQueryDataService) : base(commandArgs)
        {
            _subscriptionService = subscriptionService;
            _botSender = botSender;
            _callbackQueryDataService = callbackQueryDataService;
        }

        public override CommandTypeEnum Type => CommandTypeEnum.Command;
        protected override bool CanExecuteCommand()
        {
            return CommandArgs.Message.Text == Name || CommandArgs.Message.Text == FriendlyName;
        }

        public override async Task ExecuteCommandAsync()
        {
            var pagination = new PaginationModel();
            var model = await _subscriptionService.GetUserSubscriptionsListModelAsync(pagination, TelegramUserId);
            await _botSender.SendMessageAsync(
                new UserSubscriptionsListMessage(model, new BackNavigationArgs()
                {
                    CurrCommandData = await UserSubscriptionsListCommand.Create(pagination, _callbackQueryDataService)
                }, _callbackQueryDataService),
                ChatId, CancellationToken);
        }

        public static string CreateFriendly() => FriendlyName;
    }
}
