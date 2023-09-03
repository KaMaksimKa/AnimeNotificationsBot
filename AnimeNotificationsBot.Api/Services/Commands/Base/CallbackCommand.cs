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
            return base.CanExecute() && CanExecuteCommand();
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
            var dataId = long.Parse(CommandArgs.CallbackQuery.Data!.Split("?")[1]);
            return await CallbackQueryDataService.GetAsync<T>(dataId);
        }

        protected static async Task<string> Create<T>(string name, T data,ICallbackQueryDataService callbackQueryDataService)
        {
            var dataId = await callbackQueryDataService.AddAsync(data);
            return $"{name}?{dataId}";
        }

        /*protected static string Create(string name,Dictionary<string, string> args)
        {
            var textCommand = name;
            if (args.Any())
            {
                var isFirst = true;
                foreach (var (key,value) in args)
                {
                    textCommand += isFirst ? "?" : "&";
                    textCommand += $"{key}={value}";

                    isFirst = false;
                }
            }

            return textCommand;
        }*/

        /*private Dictionary<string,string> GetArgs()
        {
            var argsDictionary = new Dictionary<string,string>();
            var argsString = CommandArgs.CallbackQuery.Data?.Split("?").LastOrDefault();

            if (argsString != null)
            {
                var args = argsString.Split("&");
                foreach (var arg in args)
                {
                    var key = arg.Split("=")[0];
                    var value = arg.Split("=")[1];
                    argsDictionary.Add(key,value);
                }
            }

            return argsDictionary;
        }*/
    }
}
