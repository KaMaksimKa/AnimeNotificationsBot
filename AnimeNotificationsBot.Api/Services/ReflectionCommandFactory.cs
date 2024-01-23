using AnimeNotificationsBot.Api.Commands;
using AnimeNotificationsBot.Api.Commands.Base;
using AnimeNotificationsBot.Api.Commands.Base.Args;
using AnimeNotificationsBot.Api.Services.Interfaces;

namespace AnimeNotificationsBot.Api.Services
{
    public class ReflectionCommandFactory : ICommandFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ReflectionCommandFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
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
                    
                    if (_serviceProvider.GetService(y.ParameterType) is { } service)
                    {
                        return service;
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
