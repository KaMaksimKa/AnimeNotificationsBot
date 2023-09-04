using AnimeNotificationsBot.Api.Services.Commands.Base.Args;
using AnimeNotificationsBot.BLL.Interfaces;


namespace AnimeNotificationsBot.Api.Services.Commands.Base
{
    public abstract class CallbackCommand:TelegramCommand
    {
        protected CallbackCommandArgs CommandArgs;
        protected readonly ICallbackQueryDataService CallbackQueryDataService;

        protected CallbackCommand(CallbackCommandArgs commandArgs, ICallbackQueryDataService callbackQueryDataService):base(commandArgs)
        {
            CommandArgs = commandArgs;
            CallbackQueryDataService = callbackQueryDataService;
        }

        public sealed override bool CanExecute()
        {
            return base.CanExecute() && CommandArgs.CallbackQuery.Data != null && CanExecuteCommand();
        }

        protected abstract bool CanExecuteCommand();

        public sealed override async Task ExecuteAsync()
        {
            if (!CanExecute())
                throw new ArgumentException(this.GetType().Name);

            await ExecuteCommandAsync();
        }

        public abstract Task ExecuteCommandAsync();

        protected async Task<T> GetDataAsync<T>()
        {
            var dataId = long.Parse(CommandArgs.CallbackQuery.Data!.Split("&")[0].Split("?")[1]);
            return await CallbackQueryDataService.GetAsync<T>(dataId);
        }

        protected string GetBackCommand()
        {
            return CommandArgs.CallbackQuery.Data!.Split("&")[1];
        }

        protected string GetCommand()
        {
            return CommandArgs.CallbackQuery.Data!.Split("&")[0].Split("?")[0];
        }

        protected static async Task<string> Create<T>(string name, T data,ICallbackQueryDataService callbackQueryDataService, string? backCommand = null)
        {
            var dataId = await callbackQueryDataService.AddAsync(data);
            var command = $"{name}?{dataId}";
            if (backCommand != null)
                command += $"&{backCommand}";
            return command;
        }
    }
}
