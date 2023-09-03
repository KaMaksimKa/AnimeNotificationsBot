using AnimeNotificationsBot.Api.Enums;
using AnimeNotificationsBot.Api.Services.Commands.Base;
using AnimeNotificationsBot.Api.Services.Commands.Base.Args;
using AnimeNotificationsBot.Api.Services.Interfaces;
using AnimeNotificationsBot.Api.Services.Messages.Anime;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.Common.Enums;

namespace AnimeNotificationsBot.Api.Services.Commands.TelegramCommands.Anime
{
    public class AnimeInfoCommand : CallbackCommand
    {
        private readonly IAnimeService _animeService;
        private readonly IBotSender _botSender;
        private const string Name = "/anime_info";
        private readonly long _animeId;

        public AnimeInfoCommand(CallbackCommandArgs commandArgs, IAnimeService animeService, IBotSender botSender) : base(commandArgs)
        {
            _animeService = animeService;
            _botSender = botSender;
            _animeId = long.Parse(Args[nameof(_animeId)]);
        }

        public override CommandTypeEnum Type => CommandTypeEnum.TextCommand;

        protected override bool CanExecuteCommand()
        {
            return CommandArgs.CallbackQuery.Data?.StartsWith(Name) == true;
        }

        public override async Task ExecuteCommandAsync()
        {

            var anime = await _animeService.GetAnimeWithImageAsync(_animeId);

            await _botSender.SendMessageAsync(new AnimeInfoMessage(anime), ChatId,CancellationToken,CommandGroupEnum.AnimeWidget);
            await _botSender.AnswerCallbackQueryAsync(CommandArgs.CallbackQuery.Id, cancellationToken: CancellationToken);
        }


        public static string Create(long animeId)
        {
            return Create(Name,new Dictionary<string, string>()
            {
                [nameof(_animeId)] = animeId.ToString()
            });
        }
    }
}
