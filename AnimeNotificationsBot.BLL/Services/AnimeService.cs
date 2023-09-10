using AnimeNotificationsBot.BLL.Enums;
using AnimeNotificationsBot.BLL.Helpers;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.BLL.Models.Animes;
using AnimeNotificationsBot.Common.Exceptions;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AnimeNotificationsBot.BLL.Services
{
    public class AnimeService : IAnimeService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public AnimeService(DataContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AnimeModel> GetAnimeWithImageAsync(long id)
        {
            var anime = await _context.Animes.Include(x => x.Images).FirstOrDefaultAsync(x => x.Id == id);

            if (anime == null)
            {
                throw new NotFoundEntityException()
                {
                    EntityName = nameof(Anime),
                    PropertyName = nameof(anime.Id),
                    PropertyValue = id
                };
            }
            return _mapper.Map<AnimeModel>(anime);
        }

        public async Task<AnimeListModel> GetAnimeWithImageByArgsAsync(AnimeArgs args)
        {
            var animeListModel = new AnimeListModel()
            {
                Args = args
            };


            var query = _context.Animes
                .Include(x => x.Images)
                .Where(x => x.TitleRu != null);

            if (!string.IsNullOrEmpty(args.SearchQuery))
            {
                query = query.Where(x => x.TitleRu!.ToLower().Contains(args.SearchQuery.ToLower())
                                         || (x.TitleEn != null &&
                                             x.TitleEn.ToLower().Contains(args.SearchQuery.ToLower())));
            }

            animeListModel.CountAllAnime = await query.CountAsync();

            query = args.SortType switch
            {
                AnimeSortTypeEnum.Rate => args.SortOrder switch
                {
                    AnimeSortOrderEnum.Asc => query.OrderBy(x => x.Rate.HasValue ? -1 : x.Rate),
                    AnimeSortOrderEnum.Desc => query.OrderBy(x => x.Rate),
                    _ => throw new ArgumentException(nameof(AnimeSortOrderEnum))
                },
                AnimeSortTypeEnum.Name => args.SortOrder switch
                {
                    AnimeSortOrderEnum.Asc => query.OrderBy(x => x.TitleRu),
                    AnimeSortOrderEnum.Desc => query.OrderBy(x => x.TitleRu),
                    _ => throw new ArgumentException(nameof(AnimeSortOrderEnum))
                },
                _ => throw new ArgumentException(nameof(AnimeSortTypeEnum))

            };



            var animes = query
                .Skip((args.NumberOfPage - 1) * args.CountPerPage)
                .Take(args.CountPerPage)
                .ToList();

   


            animeListModel.Animes = animes.Select(x => _mapper.Map<AnimeModel>(x)).ToList();

            return animeListModel;
        }

        public async Task<AnimeInfoModel> GetAnimeInfoModel(long animeId)
        {
            var anime = await GetAnimeWithImageAsync(animeId);
            var showNotification = await _context.AnimeNotifications
                .AnyAsync(
                    x => x.AnimeId == animeId && x.CreatedDate > DateTimeOffset.UtcNow.AddDays(-14));

            return new AnimeInfoModel()
            {
                Anime = anime,
                ShowNotification = showNotification
            };

        }
    }
}
