using AnimeNotificationsBot.Api.Services.Commands;
using AnimeNotificationsBot.Api.Services.Commands.Base;
using AnimeNotificationsBot.Api.Services.Commands.Base.Args;
using AnimeNotificationsBot.Api.Services.Commands.TelegramCommands;
using AnimeNotificationsBot.Api.Services.Commands.TelegramCommands.Anime;
using AnimeNotificationsBot.Api.Services.Commands.TelegramCommands.Subscriptions;
using AnimeNotificationsBot.Api.Services.Interfaces;
using AnimeNotificationsBot.BLL.Interfaces;

namespace AnimeNotificationsBot.Api.Services
{
    public class CommandFactory : ICommandFactory
    {
        private readonly IUserService _userService;
        private readonly IBotSender _botSender;
        private readonly IAnimeService _animeService;
        private readonly ICallbackQueryDataService _callbackQueryDataService;
        private readonly IAnimeSubscriptionService _subscriptionService;
        private readonly IDubbingService _dubbingService;

        public CommandFactory(IUserService userService, IBotSender botSender, IAnimeService animeService,
            ICallbackQueryDataService callbackQueryDataService,IAnimeSubscriptionService subscriptionService,
            IDubbingService dubbingService)
        {
            _userService = userService;
            _botSender = botSender;
            _animeService = animeService;
            _callbackQueryDataService = callbackQueryDataService;
            _subscriptionService = subscriptionService;
            _dubbingService = dubbingService;
        }

        public ICommand CreateCallbackCommand(CallbackCommandArgs commandArgs)
        {
            var command = new CombiningCommand(new ICallbackCommand[]
            {
                new AnimeInfoCommand(commandArgs,_animeService,_botSender,_callbackQueryDataService),
                new AnimeListCommand(commandArgs,_animeService,_botSender,_callbackQueryDataService),
                new SubscribedAnimeCommand(commandArgs,_callbackQueryDataService,_subscriptionService,_botSender,_animeService),
                new СhoseAnimeForSubCommand(commandArgs,_callbackQueryDataService,_animeService,_subscriptionService,_botSender)
            });

            return new MainCommand(commandArgs, command, _botSender);
        }

        public ICommand CreateMessageCommand(MessageCommandArgs commandArgs)
        {
            var command = new CombiningCommand(new IMessageCommand[]
            {
                new StartCommand(commandArgs,_userService,_botSender),
                new HelpCommand(commandArgs,_botSender),
                new FindAnimeCommand(commandArgs,_userService,_botSender),
                new FoundAnimeCommand(commandArgs,_animeService,_userService,_botSender,_callbackQueryDataService),
                new CancelCommand(commandArgs,_userService,_botSender),
                new MenuCommand(commandArgs,_botSender)
            });

            return new MainCommand(commandArgs, command, _botSender);
        }
    }
}
