namespace AnimeNotificationsBot.Api.Services.Commands.Base
{
    public interface ICommand
    {
        public bool CanExecute();
        public Task ExecuteAsync();
    }
}
