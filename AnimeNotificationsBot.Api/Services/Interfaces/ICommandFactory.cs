using AnimeNotificationsBot.Api.Services.Commands.Base;

namespace AnimeNotificationsBot.Api.Services.Interfaces
{
    public interface ICommandFactory
    {
        public ITelegramCommand CreateCallbackCommand(CallbackCommandArgs commandArgs);
        public ITelegramCommand CreateMessageCommand(MessageCommandArgs commandArgs);
    }
}
