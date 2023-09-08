using AnimeNotificationsBot.BLL.Enums;
using AnimeNotificationsBot.BLL.Helpers;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.BLL.Models.Anime;
using AnimeNotificationsBot.Common.Exceptions;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace AnimeNotificationsBot.BLL.Services
{
    public class AnimeService : IAnimeService
    {
        private readonly DataContext _context;
        private readonly IImageService _imageService;

        public AnimeService(DataContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public async Task<AnimeWithImageModel> GetAnimeWithImageAsync(long id)
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
            var image = anime.Images.FirstOrDefault();
            return new AnimeWithImageModel()
            {
                Id = anime.Id,
                TitleRu = anime.TitleRu!,
                Rate = anime.Rate,
                Image = image == null ? null : await _imageService.GetImage(image.Path),
            };
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

            var animeModels = new List<AnimeWithImageModel>();

            foreach (var anime in animes)
            {
                var image = anime.Images.FirstOrDefault();
                animeModels.Add(new AnimeWithImageModel
                {
                    Id = anime.Id,
                    TitleRu = anime.TitleRu!,
                    Rate = anime.Rate,
                    Image = image == null ? null : await _imageService.GetImage(image.Path),
                });
            }

            animeListModel.Animes = animeModels;

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
