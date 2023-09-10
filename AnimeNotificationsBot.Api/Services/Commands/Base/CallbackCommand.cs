using AnimeNotificationsBot.Api.Models;
using AnimeNotificationsBot.Api.Services.Commands.Base.Args;
using AnimeNotificationsBot.Api.Services.Messages.Base;
using AnimeNotificationsBot.BLL.Interfaces;


namespace AnimeNotificationsBot.Api.Services.Commands.Base
{
    public abstract class CallbackCommand<TCommand,TArgs>:TelegramCommand,ICallbackCommand
    {
        protected CallbackCommandArgs CommandArgs;
        protected readonly ICallbackQueryDataService CallbackQueryDataService;
        protected static readonly string Name = typeof(TCommand).Name;

        private CallbackDataModel<TArgs>? _data;


        protected CallbackCommand(CallbackCommandArgs commandArgs, ICallbackQueryDataService callbackQueryDataService):base(commandArgs)
        {
            CommandArgs = commandArgs;
            CallbackQueryDataService = callbackQueryDataService;
        }

        public sealed override bool CanExecute()
        {
            return base.CanExecute() && GetCommandNameFromQuery() == Name && CanExecuteCommand();
        }

        protected virtual bool CanExecuteCommand() => true;

        public sealed override async Task ExecuteAsync()
        {
            if (!CanExecute())
                throw new ArgumentException(this.GetType().Name);

            await ExecuteCommandAsync();
        }

        public abstract Task ExecuteCommandAsync();

        public static async Task<string> Create(TArgs data,ICallbackQueryDataService callbackQueryDataService, string? backCommand = null)
        {
            var dataId = await callbackQueryDataService.AddAsync(new CallbackDataModel<TArgs>()
            {
                Data = data,
                PrevStringCommand = backCommand
            });

            return $"{Name}?{dataId}";
        }

        protected async Task<CallbackDataModel<TArgs>> GetDataAsync()
        {
            if (_data == null)
            {
                var dataId = long.Parse(CommandArgs.CallbackQuery.Data!.Split("&")[0].Split("?")[1]);
                _data = await CallbackQueryDataService.GetAsync<CallbackDataModel<TArgs>>(dataId);
            }

            return _data;
        }

        protected string GetCurrCommandFromQuery()
        {
            return CommandArgs.CallbackQuery.Data!.Split("&")[0];
        }

        protected string GetCommandNameFromQuery()
        {
            return CommandArgs.CallbackQuery.Data!.Split("&")[0].Split("?")[0];
        }

        protected async Task<BackNavigationArgs> GetBackNavigationArgs()
        {
            return new BackNavigationArgs()
            {
                CurrCommandData = GetCurrCommandFromQuery(),
                PrevCommandData = (await GetDataAsync()).PrevStringCommand
            };
        }
    }
}
