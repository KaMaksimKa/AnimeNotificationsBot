using AnimeNotificationsBot.Api.Services.Commands.Base;
using AnimeNotificationsBot.Api.Services.Commands.Base.Args;

namespace AnimeNotificationsBot.Api.Services.Interfaces
{
    public interface ICommandFactory
    {
        public ICommand CreateCallbackCommand(CallbackCommandArgs commandArgs);
        public ICommand CreateMessageCommand(MessageCommandArgs commandArgs);
    }
}
