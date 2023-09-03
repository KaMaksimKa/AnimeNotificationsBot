using AnimeNotificationsBot.Api.Services.Commands.Base.Args;

namespace AnimeNotificationsBot.Api.Services.Commands.Base
{
    public abstract class CallbackCommand:TelegramCommand
    {
        protected CallbackCommandArgs CommandArgs;

        protected Dictionary<string, string> Args;
        protected CallbackCommand(CallbackCommandArgs commandArgs):base(commandArgs)
        {
            CommandArgs = commandArgs;
            Args = GetArgs();
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

        protected static string Create(string name,Dictionary<string, string> args)
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
        }

        private Dictionary<string,string> GetArgs()
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
        }
    }
}
