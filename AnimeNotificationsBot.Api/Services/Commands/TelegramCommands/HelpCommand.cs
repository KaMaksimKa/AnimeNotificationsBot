﻿using AnimeNotificationsBot.Api.Enums;
using AnimeNotificationsBot.Api.Services.Commands.Base;
using AnimeNotificationsBot.Api.Services.Commands.Base.Args;
using AnimeNotificationsBot.Api.Services.Interfaces;
using AnimeNotificationsBot.Api.Services.Messages;

namespace AnimeNotificationsBot.Api.Services.Commands.TelegramCommands
{
    public class HelpCommand : MessageCommand
    {
        private readonly IBotSender _botSender;
        private const string Name = "/help";
        private const string FriendlyName = "Помощь";

        public HelpCommand(MessageCommandArgs commandArgs, IBotSender botSender) : base(commandArgs)
        {
            _botSender = botSender;
        }

        public override CommandTypeEnum Type => CommandTypeEnum.TextCommand;

        protected override bool CanExecuteCommand()
        {
            return CommandArgs.Message.Text == Name || CommandArgs.Message.Text == FriendlyName;
        }

        public override async Task ExecuteCommandAsync()
        {
            await _botSender.SendMessageAsync(new HelpMessage(), CommandArgs.Message.Chat.Id, CommandArgs.CancellationToken);
        }

        public static string Create()
        {
            return $"{Name}";
        }

        public static string CreateFriendly()
        {
            return $"{FriendlyName}";
        }
    }
}