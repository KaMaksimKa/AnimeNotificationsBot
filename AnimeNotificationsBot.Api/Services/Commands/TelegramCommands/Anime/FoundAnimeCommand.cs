using AnimeNotificationsBot.Api.Enums;
using AnimeNotificationsBot.Api.Models;
using AnimeNotificationsBot.Api.Services.Commands.Base;
using AnimeNotificationsBot.Api.Services.Commands.Base.Args;
using AnimeNotificationsBot.Api.Services.Interfaces;
using AnimeNotificationsBot.Api.Services.Messages.Anime;
using AnimeNotificationsBot.Api.Services.Messages.Base;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.Common.Enums;

namespace AnimeNotificationsBot.Api.Services.Commands.TelegramCommands.Anime
{
    public class FoundAnimeCommand : MessageCommand
    {
        private const int MaxFoundAnimeCount = 10;

        private readonly IAnimeService _animeService;
        private readonly IUserService _userService;
        private readonly IBotSender _botSender;

        public FoundAnimeCommand(MessageCommandArgs commandArgs, IAnimeService animeService, IUserService userService,
            IBotSender botSender) : base(commandArgs)
        {
            _animeService = animeService;
            _userService = userService;
            _botSender = botSender;
        }

        public override CommandTypeEnum Type => CommandTypeEnum.TextAnswer;

        protected override bool CanExecuteCommand()
        {
            return _userService.GetCommandStateAsync(TelegramUserId).Result == CommandStateEnum.FindAnime;
        }

        public override async Task ExecuteCommandAsync()
        {
            var animes = await _animeService.GetAnimesWithImagesAsync(CommandArgs.Message.Text);

            if (animes.Any())
            {
                var isAll = animes.Count <= MaxFoundAnimeCount;
                if (!isAll)
                    animes = animes.Take(MaxFoundAnimeCount).ToList();

                await _userService.SetCommandStateAsync(TelegramUserId, CommandStateEnum.None);

                var mediaGroupMessage = new MediaGroupMessage()
                {
                    Images = animes
                        .Where(x => x.Image != null)
                        .Select(x => new TelegramPhotoModel()
                        {
                            Caption = x.TitleRu,
                            Image = x.Image!
                        }).ToList()
                };

                await _botSender.SendMessageAsync(mediaGroupMessage, ChatId, CommandArgs.CancellationToken,CommandGroupEnum.AnimeWidget);

                await _botSender.SendMessageAsync(new FoundAnimeMessage(animes, isAll), ChatId, CommandArgs.CancellationToken);
            }
            else
            {
                await _botSender.SendMessageAsync(new NotFoundAnimeMessage(), ChatId, CommandArgs.CancellationToken);
            }
        }
    }
}
