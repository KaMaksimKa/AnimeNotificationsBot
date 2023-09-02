using AnimeNotificationsBot.Api.Models;
using AnimeNotificationsBot.Api.Services.Commands.Base;
using AnimeNotificationsBot.Api.Services.Interfaces;
using AnimeNotificationsBot.Api.Services.Messages.Anime;
using AnimeNotificationsBot.Api.Services.Messages.Base;
using AnimeNotificationsBot.BLL.Interfaces;

namespace AnimeNotificationsBot.Api.Services.Commands.Anime
{
    public class FindAnimeCommand : MessageCommand
    {
        private readonly IAnimeService _animeService;
        private readonly IBotSender _botSender;
        private const string Name = "/find_anime";
        private const string FriendlyName = "Найти аниме";

        public FindAnimeCommand(MessageCommandArgs commandArgs, IAnimeService animeService, IBotSender botSender) : base(commandArgs)
        {
            _animeService = animeService;
            _botSender = botSender;
        }

        public override bool CanExecute()
        {
            var text = CommandArgs.Message.Text;

            return text == Name || text == FriendlyName;
        }

        public override async Task ExecuteAsync()
        {
            if (!CanExecute())
            {
                throw new ArgumentException();
            }

            var animes = await _animeService.GetAnimesWithImagesAsync();

            await _botSender.SendMessageAsync(new MediaGroupMessage()
            {
                Images = animes
                    .Where(x => x.Image != null)
                    .Select(x => new TelegramPhotoModel()
                    {
                        Caption = x.TitleRu,
                        Image = x.Image!
                    }).ToList()
            }, CommandArgs.Message.Chat.Id, CommandArgs.CancellationToken);

            await _botSender.SendMessageAsync(new FoundAnimeMessage(animes),CommandArgs.Message.Chat.Id,CommandArgs.CancellationToken);
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
