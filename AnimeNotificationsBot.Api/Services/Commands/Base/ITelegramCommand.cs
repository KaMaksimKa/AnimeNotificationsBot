namespace AnimeNotificationsBot.Api.Services.Commands.Base
{
    public interface ITelegramCommand
    {
        public bool CanExecute();
        public Task ExecuteAsync();
    }
}
