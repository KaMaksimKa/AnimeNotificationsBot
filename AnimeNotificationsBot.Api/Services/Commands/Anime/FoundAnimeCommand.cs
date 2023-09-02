using AnimeNotificationsBot.Api.Services.Commands.Base;
using AnimeNotificationsBot.Api.Services.Interfaces;
using AnimeNotificationsBot.Api.Services.Messages.Base;
using AnimeNotificationsBot.BLL.Interfaces;

namespace AnimeNotificationsBot.Api.Services.Commands.Anime
{
    public class FoundAnimeCommand: MessageCommand
    {
        private readonly IAnimeService _animeService;
        private readonly IBotSender _botSender;

        public FoundAnimeCommand(MessageCommandArgs commandArgs, IAnimeService animeService, IBotSender botSender) : base(commandArgs)
        {
            _animeService = animeService;
            _botSender = botSender;
        }

        public override bool CanExecute()
        {
            return false;
        }

        public override async Task ExecuteAsync()
        {
            if (CanExecute())
                throw new ArgumentException(nameof(FoundAnimeCommand));


        }
    }
}
