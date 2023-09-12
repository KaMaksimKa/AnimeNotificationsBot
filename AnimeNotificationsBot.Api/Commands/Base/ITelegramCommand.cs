using AnimeNotificationsBot.Api.Enums;

namespace AnimeNotificationsBot.Api.Commands.Base
{
    public interface ITelegramCommand : ICommand
    {
        public CommandTypeEnum Type { get; }
    }
}
