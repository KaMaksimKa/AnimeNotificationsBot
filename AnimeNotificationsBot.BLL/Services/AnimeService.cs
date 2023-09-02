using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.BLL.Models.Anime;
using AnimeNotificationsBot.Common.Exceptions;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

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

        public async Task<List<AnimeWithImageModel>> GetAnimesWithImagesAsync(string? searchQuery = null, int numberOfPage = 1, int quantity = 5)
        {
            var animes = await _context.Animes
                .Include(x => x.Images)
                .Where(x => x.TitleRu != null)
                .Where(x => searchQuery == null || (x.TitleRu != null && x.TitleRu.Contains(searchQuery))
                    || (x.TitleEn != null && x.TitleEn.Contains(searchQuery)))
                .Skip((numberOfPage - 1) * quantity)
                .Take(quantity)
                .ToListAsync();

            var animeModels = new List<AnimeWithImageModel>();

            foreach (var anime in animes)
            {
                var image = anime.Images.FirstOrDefault();
                animeModels.Add(new AnimeWithImageModel
                {
                    Id = anime.Id,
                    TitleRu = anime.TitleRu!,
                    Image = image == null ? null : await _imageService.GetImage(image.Path),
                });
            }

            return animeModels;
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
                Image = image == null ? null : await _imageService.GetImage(image.Path),
            };
        }
    }
}
