using AnimeNotificationsBot.Api.Commands;
using AnimeNotificationsBot.Api.Commands.Base;
using AnimeNotificationsBot.Api.Commands.Base.Args;
using AnimeNotificationsBot.Api.Commands.TelegramCommands;
using AnimeNotificationsBot.Api.Commands.TelegramCommands.Animes;
using AnimeNotificationsBot.Api.Commands.TelegramCommands.Feedbacks;
using AnimeNotificationsBot.Api.Commands.TelegramCommands.Subscriptions;
using AnimeNotificationsBot.Api.Services.Interfaces;
using AnimeNotificationsBot.BLL.Interfaces;
using System.Reflection.Metadata;
using System.Reflection;
using System.Security.AccessControl;

namespace AnimeNotificationsBot.Api.Services
{
    public class ReflectionCommandFactory : ICommandFactory
    {
        private readonly Dictionary<string, object> _services;

        public ReflectionCommandFactory(IUserService userService, IBotSender botSender, IAnimeService animeService,
            ICallbackQueryDataService callbackQueryDataService, IAnimeSubscriptionService subscriptionService,
            IFeedbackService feedbackService)
        {
            _services = new Dictionary<string, object>()
            {
                [nameof(IUserService)] = userService,
                [nameof(IBotSender)] = botSender,
                [nameof(IAnimeService)] = animeService,
                [nameof(ICallbackQueryDataService)] = callbackQueryDataService,
                [nameof(IAnimeSubscriptionService)] = subscriptionService,
                [nameof(IFeedbackService)] = feedbackService,
            };
        }

        public ICommand CreateCallbackCommand(CallbackCommandArgs commandArgs)
        {
            return CreateCommand(typeof(CallbackCommand), commandArgs);
        }

        public ICommand CreateMessageCommand(MessageCommandArgs commandArgs)
        {
            return CreateCommand(typeof(MessageCommand), commandArgs);
        }

        private ICommand CreateCommand(Type baseCommandType, object commandArgs)
        {
            var assembly = typeof(ReflectionCommandFactory).Assembly;
            var derivedTypes = assembly.GetTypes().Where(x => baseCommandType.IsAssignableFrom(x) && x != baseCommandType && !x.IsAbstract);

            var combiningCommand = new CombiningCommand(derivedTypes.Select(x =>
            {
                var parameters = x.GetConstructors().FirstOrDefault()?.GetParameters();
                if (parameters == null)
                    throw new InvalidOperationException("В классе команды должен быть хотя бы один конструктор.");

                object[] paramObjects = parameters.Select(y =>
                {
                    if (_services.ContainsKey(y.ParameterType.Name))
                    {
                        return _services[y.ParameterType.Name];
                    }
                    else if (y.ParameterType.Name == commandArgs.GetType().Name)
                    {
                        return commandArgs;
                    }
                    else
                    {
                        throw new ArgumentException($"Конструкор требует недопустимый параметр {y.ParameterType.Name}");
                    }
                }).ToArray();

                var command = Activator.CreateInstance(x, paramObjects);

                if (command is ITelegramCommand telegramCommand)
                {
                    return telegramCommand;
                }
                else
                {
                    throw new ArgumentException($"Неизвестный тип: {command?.GetType().Name}");
                }
            }));

            return combiningCommand;
        }
    }
}
