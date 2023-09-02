using AnimeNotificationsBot.Api.Models;
using AnimeNotificationsBot.Api.Services.Commands.Base;
using AnimeNotificationsBot.Api.Services.Interfaces;
using AnimeNotificationsBot.Api.Services.Messages.Anime;
using AnimeNotificationsBot.Api.Services.Messages.Base;
using AnimeNotificationsBot.BLL.Interfaces;

namespace AnimeNotificationsBot.Api.Services.Commands.Anime
{
    public class AnimeInfoCommand: CallbackCommand
    {
        private readonly IAnimeService _animeService;
        private readonly IBotSender _botSender;
        private const string Name = "/anime_info";

        public AnimeInfoCommand(CallbackCommandArgs commandArgs, IAnimeService animeService, IBotSender botSender) : base(commandArgs)
        {
            _animeService = animeService;
            _botSender = botSender;
        }

        public override bool CanExecute()
        {
            if (CommandArgs.CallbackQuery.Message == null)
                return false;
            
            return CommandArgs.CallbackQuery.Data?.StartsWith(Name + " ") == true;
        }

        public override async Task ExecuteAsync()
        {
            if (!CanExecute())
                throw new ArgumentException(nameof(AnimeInfoCommand));

            var animeId = GetAnimeId();

            var anime = await _animeService.GetAnimeWithImageAsync(animeId);

            await _botSender.SendMessageAsync(new AnimeInfoMessage(anime), CommandArgs.CallbackQuery.Message!.Chat.Id);
        }

        private long GetAnimeId()
        {
            var animeIdString = CommandArgs.CallbackQuery.Data!.Split()[1];
            return long.Parse(animeIdString);
        }

        public static string Create(long animeId)
        {
            return $"{Name} {animeId}";
        }
    }
}
