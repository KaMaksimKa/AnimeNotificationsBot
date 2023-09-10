using AnimeNotificationsBot.Api.Enums;

namespace AnimeNotificationsBot.Api.Services.Commands.Base
{
    public interface ITelegramCommand: ICommand
    {
        public CommandTypeEnum Type { get; }
    }
}
