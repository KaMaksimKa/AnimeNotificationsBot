using AnimeNotificationsBot.BLL.Helpers;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.BLL.Models;
using AnimeNotificationsBot.BLL.Models.Animes;
using AnimeNotificationsBot.BLL.Models.Dubbing;
using AnimeNotificationsBot.BLL.Models.Subscriptions;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace AnimeNotificationsBot.BLL.Services
{
    public class AnimeSubscriptionService : IAnimeSubscriptionService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public AnimeSubscriptionService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<SubscribedAnimeModel> SubscribeAsync(SubscribeAnimeModel model, long telegramUserId)
        {

            var user = await _context.Users.GetUserByTelegramId(telegramUserId);

            var subs = await _context.Animes
                .Where(x => (!model.AnimeId.HasValue || model.AnimeId == x.Id))
                .SelectMany(x => x.DubbingFromFirstEpisode.Select(y => new
                {
                    AnimeId = x.Id,
                    AnimeTitle = x.TitleRu,
                    DubbingId = y.Id,
                    DubbingTite = y.Title

                }))
                .Where(x => !model.DubbingId.HasValue || model.DubbingId == x.DubbingId)
                .ToListAsync();

            await _context.AnimeSubscriptions.AddRangeAsync(subs.Select(x => new AnimeSubscription()
            {
                AnimeId = x.AnimeId,
                DubbingId = x.DubbingId,
                User = user,
                CreatedDate = DateTimeOffset.Now.ToUniversalTime(),
            }));

            await _context.SaveChangesAsync();

            return new SubscribedAnimeModel()
            {
                AnimeId = model.AnimeId,
                AnimeTitle = model.AnimeId.HasValue ? subs.FirstOrDefault()?.AnimeTitle : null,
                DubbingId = model.DubbingId,
                DubbingTitle = model.DubbingId.HasValue ? subs.FirstOrDefault()?.DubbingTite : null,
                SubscriptionsCount = subs.Count,
            };

        }

        public async Task<SubscribedAnimeModel> UnsubscribeAsync(SubscribeAnimeModel model, long telegramUserId)
        {

            var count = await _context.AnimeSubscriptions
                .Where(x => (!model.AnimeId.HasValue || model.AnimeId == x.AnimeId)
                            && (!model.DubbingId.HasValue || model.DubbingId == x.DubbingId)
                            && telegramUserId == x.User.TelegramUserId)
                .ExecuteUpdateAsync(x => x.SetProperty(y => y.IsRemoved, y => true));

            return new SubscribedAnimeModel()
            {
                AnimeId = model.AnimeId,
                AnimeTitle = model.AnimeId.HasValue ? (await _context.Animes.FindAsync(model.AnimeId))?.TitleRu : null,
                DubbingId = model.DubbingId,
                DubbingTitle = model.DubbingId.HasValue
                    ? (await _context.Dubbing.FindAsync(model.AnimeId))?.Title
                    : null,
                SubscriptionsCount = count
            };
        }

        public async Task<List<SubscriptionDubbingModel>> GetUserSubscriptionsByAnimeAsync(long animeId, long telegramUserId)
        {
            var user = await _context.Users.GetUserByTelegramId(telegramUserId);

            var subs = await _context.Animes
                .Where(x => x.Id == animeId)
                .SelectMany(x => x.DubbingFromFirstEpisode)
                .Select(x => new SubscriptionDubbingModel()
                {
                    AnimeId = animeId,
                    Dubbing = new DubbingModel()
                    {
                        Id = x.Id,
                        Title = x.Title,
                    },
                    HasSubscribe = x.AnimeSubscriptions.Any(y => !y.IsRemoved && y.AnimeId == animeId && y.UserId == user.Id)
                }).ToListAsync();

            return subs;
        }

        public async Task<UserSubscriptionListModel> GetUserSubscriptionsListModelAsync(PaginationModel pagination,long telegramUserId)
        {
            var user = await _context.Users.GetUserByTelegramId(telegramUserId);

            var query = _context.AnimeSubscriptions
                .Where(x => !x.IsRemoved
                            && x.UserId == user.Id
                            && x.Anime.AnimeNotifications.Any(y => y.CreatedDate > DateTimeOffset.UtcNow.AddDays(-14)))
                .Select(x => x.Anime)
                .Distinct();
                

            return new UserSubscriptionListModel()
            {
                CountAllAnime = query.Count(),
                Pagination = pagination,
                Animes = query
                    .Skip((pagination.NumberOfPage-1)*pagination.CountPerPage)
                    .Take(pagination.CountPerPage)
                    .ProjectTo<AnimeModel>(_mapper.ConfigurationProvider)
                    .ToList(),
        };
        }
    }
}
