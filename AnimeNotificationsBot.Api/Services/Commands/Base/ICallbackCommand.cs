namespace AnimeNotificationsBot.Api.Services.Commands.Base
{
    public interface ICallbackCommand
    {
        static abstract Task<string> Create<T>(T arg);
    }
}
