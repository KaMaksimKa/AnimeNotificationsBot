namespace AnimeNotificationsBot.Api.Commands.Base
{
    public interface ICommand
    {
        public bool CanExecute();
        public Task ExecuteAsync();
    }
}
