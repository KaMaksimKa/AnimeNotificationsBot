using AnimeNotificationsBot.Api.Commands.Base;
using AnimeNotificationsBot.Api.Commands.Base.Args;
using AnimeNotificationsBot.Api.Enums;
using AnimeNotificationsBot.Api.Messages;
using AnimeNotificationsBot.Api.Services.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace AnimeNotificationsBot.Api.Commands.TelegramCommands
{
    public class TestCommand : MessageCommand
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IBotSender _botSender;

        public TestCommand(ITelegramBotClient telegramBotClient,IBotSender botSender ,MessageCommandArgs commandArgs) : base(commandArgs)
        {
            _botClient = telegramBotClient;
            _botSender = botSender;
        }

        public override CommandTypeEnum Type => CommandTypeEnum.Command;

        public async override Task ExecuteCommandAsync()
        {
            await _botClient.SendTextMessageAsync(CommandArgs.Message.Chat.Id, "Test", replyMarkup: new ReplyKeyboardMarkup(KeyboardButton.WithWebApp("Test", new WebAppInfo() { Url = "https://ya.ru" })),cancellationToken:CommandArgs.CancellationToken);
        }

        protected override bool CanExecuteCommand()
        {
            return CommandArgs.Message.Text == "/test";
        }
    }
}
