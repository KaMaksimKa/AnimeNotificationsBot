using AnimeNotificationsBot.Api.Commands.Base.Args;
using AnimeNotificationsBot.Api.Messages.Base;
using AnimeNotificationsBot.Api.Models;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.BLL.Services;


namespace AnimeNotificationsBot.Api.Commands.Base
{
    public abstract class CallbackCommand<TCommand, TArgs> : CallbackCommand, ICallbackCommand
    {
        protected CallbackCommandArgs CommandArgs;
        protected readonly ICallbackQueryDataService CallbackQueryDataService;
        protected static readonly string Name = typeof(TCommand).Name;

        private CallbackData<TArgs>? _data;


        protected CallbackCommand(CallbackCommandArgs commandArgs, ICallbackQueryDataService callbackQueryDataService) : base(commandArgs)
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
                throw new ArgumentException(GetType().Name);

            await ExecuteCommandAsync();
        }

        public abstract Task ExecuteCommandAsync();

        public static async Task<string> Create(TArgs data, ICallbackQueryDataService callbackQueryDataService, string? backCommand = null)
        {
            var dataId = await callbackQueryDataService.AddAsync(new CallbackData<TArgs>()
            {
                Data = data,
                StringBackCommand = backCommand
            });

            return $"{Name}?{dataId}";
        }

        protected async Task<CallbackData<TArgs>> GetDataAsync()
        {
            if (_data == null)
            {
                var dataId = long.Parse(CommandArgs.CallbackQuery.Data!.Split("&")[0].Split("?")[1]);
                _data = await CallbackQueryDataService.GetAsync<CallbackData<TArgs>>(dataId);
            }

            return _data;
        }

        protected string GetCommandNameFromQuery()
        {
            return CommandArgs.CallbackQuery.Data!.Split("?")[0];
        }

        protected async Task<BackNavigationArgs> GetBackNavigationArgs()
        {
            return new BackNavigationArgs()
            {
                CurrCommandData = CommandArgs.CallbackQuery.Data!,
                PrevCommandData = (await GetDataAsync()).StringBackCommand
            };
        }
    }

    public abstract class CallbackCommand: TelegramCommand {

        protected CallbackCommand(CallbackCommandArgs commandArgs) : base(commandArgs)
        {

        }
    }
}
