using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.Common.Interfaces;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ParserAnimeGO;
using ParserAnimeGO.Models;

namespace AnimeNotificationsBot.Quartz.Services
{
    public class AnimeService
    {
        private readonly ParserAnimeGo _parser;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;


        public AnimeService(ParserAnimeGo parser, DataContext context, IMapper mapper, IImageService imageService)
        {
            _parser = parser;
            _context = context;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task UpdateNotificationsAsync()
        {
            var notificationsFromParser = (await _parser.GetAnimeNotificationsFromAnimeGoAsync())
                .Where(x => x.IdFromAnimeGo.HasValue && !string.IsNullOrEmpty(x.Dubbing))
                .ToList();

            var animes = await _context.Animes
                .Where(x => notificationsFromParser
                    .Select(y => y.IdFromAnimeGo)
                    .Contains(x.IdFromAnimeGo))
                .ToListAsync();

            var dubbing = await _context.Dubbing
                .Where(x => notificationsFromParser
                    .Select(y => y.Dubbing)
                    .Contains(x.Title))
                .ToListAsync();

            foreach (var notificationFromParser in notificationsFromParser)
            {
                var anime = animes.FirstOrDefault(x => x.IdFromAnimeGo == notificationFromParser.IdFromAnimeGo);
                var dub = dubbing.FirstOrDefault(x => x.Title == notificationFromParser.Dubbing);
                var notification = await _context.AnimeNotifications
                        .Where(x => notificationFromParser.IdFromAnimeGo == x.Anime.IdFromAnimeGo
                                    && notificationFromParser.Dubbing == x.Dubbing.Title
                                    && notificationFromParser.SerialNumber == x.SerialNumber)
                        .FirstOrDefaultAsync();

                if (anime != null && dub != null && notification == null)
                {
                    await _context.AnimeNotifications.AddAsync(new AnimeNotification()
                    {
                        SerialNumber = notificationFromParser.SerialNumber,
                        Dubbing = dub,
                        Anime = anime,
                        CreatedDate = DateTimeOffset.Now.ToUniversalTime(),
                        IsNotified = false
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAllCommentsAsync()
        {
            int numberOfPage = 1;
            int takeCount = 5;
            List<Anime> animes;
            do
            {
                animes = await _context.Animes
                    .Include(y => y.Comments)
                    .OrderBy(y => y.Id)
                    .Skip((numberOfPage - 1) * takeCount)
                    .Take(takeCount)
                    .ToListAsync();

                foreach (var anime in animes)
                {
                    if (!anime.IdForComments.HasValue)
                        continue;

                    var commentsFromParser = await _parser.GetAllCommentsFromAnime(anime.IdForComments.Value, 200);
                    var comments = commentsFromParser.Select(x => _mapper.Map<AnimeComment>(x)).ToList();

                    if (comments.Count != comments.Select(x => x.Id).Distinct().Count() || comments.Count == 0)
                    {

                    }
                    anime.Comments.Clear();
                    anime.Comments.AddRange(comments);
                }

                await _context.SaveChangesAsync();

                numberOfPage++;
            } while (animes.Any());
        }

        public async Task UpdateAllAnimesAsync()
        {
            List<AnimeFromParser> animesFromParser;
            var numberOfPage = 1;
            do
            {
                animesFromParser = await GetAnimesByPage(numberOfPage);
                var animes = animesFromParser.Select(x => _mapper.Map<Anime>(x)).ToList();
                var preparedAnimes = await PrepareAnimesForAddToContext(animes);
                var preparedAnimesWithImages = await DownloadNewAnimeImagesAsync(preparedAnimes);
                await AddOrUpdateAnimesInContext(preparedAnimesWithImages);

                numberOfPage++;

            } while (animesFromParser.Any());

            
        }

        private async Task<List<AnimeFromParser>> GetAnimesByPage(int numberOfPage) =>
            await _parser.GetFullAnimesByArgsAsync(new AnimesArgs()
            {
                PageNumber = numberOfPage
            });

        private async Task<List<Anime>> PrepareAnimesForAddToContext(List<Anime> animes)
        {
            var genres = await _context.Genres.ToListAsync();
            var types = await _context.AnimeTypes.ToListAsync();
            var statuses = await _context.AnimeStatuses.ToListAsync();
            var studios = await _context.Studios.ToListAsync();
            var dubbing = await _context.Dubbing.ToListAsync();
            var mpaaRates = await _context.MpaaRates.ToListAsync();

            foreach (var anime in animes)
            {
                anime.Type = anime.Type == null ? null : await PrepareForAddToContext(anime.Type, types);
                anime.Status = anime.Status == null ? null : await PrepareForAddToContext(anime.Status, statuses);
                anime.MpaaRate = anime.MpaaRate == null ? null : await PrepareForAddToContext(anime.MpaaRate, mpaaRates);
                anime.Genres = await PrepareForAddToContext(anime.Genres, genres);
                anime.Studios = await PrepareForAddToContext(anime.Studios, studios);
                anime.Dubbing = await PrepareForAddToContext(anime.Dubbing, dubbing);
                anime.DubbingFromFirstEpisode = await PrepareForAddToContext(anime.DubbingFromFirstEpisode, dubbing);
            }

            return animes;
        }

        private async Task<T> PrepareForAddToContext<T>(T entity, List<T> oldEntities) where T : class, ITitleEntity
        {
            var _entity = oldEntities.FirstOrDefault(x => x.Title == entity.Title);

            if (_entity == null)
            {
                await _context.Set<T>().AddAsync(entity);
                oldEntities.Add(entity);
                return entity;
            }
            else
                return _entity;
        }

        private async Task<List<T>> PrepareForAddToContext<T>(List<T> entities, List<T> oldEntities) where T : class, ITitleEntity
        {
            var newEntities = new List<T>();
            foreach (var entity in entities)
            {
                newEntities.Add(await PrepareForAddToContext(entity, oldEntities));
            }

            return newEntities;
        }

        private async Task AddOrUpdateAnimesInContext(List<Anime> animes)
        {
            #region Подготовка Animes

            var _animes = await _context.Animes
                .Include(x => x.Genres)
                .Include(x => x.Dubbing)
                .Include(x => x.DubbingFromFirstEpisode)
                .Include(x => x.Studios)
                .Include(x => x.Images)
                .Where(x => animes
                    .Select(y => y.IdFromAnimeGo)
                    .Contains(x.IdFromAnimeGo))
                .ToListAsync();


            foreach (var _anime in _animes)
            {
                _anime.Genres.Clear();
                _anime.Dubbing.Clear();
                _anime.Studios.Clear();
                _anime.DubbingFromFirstEpisode.Clear();
                _anime.Images.Clear();
            }

            #endregion

            foreach (var anime in animes)
            {
                if (_animes.FirstOrDefault(x => anime.IdFromAnimeGo == x.IdFromAnimeGo) is
                    { } animeFromDb)
                {
                    _mapper.Map(anime, animeFromDb);
                    _context.Animes.Update(animeFromDb);
                }
                else
                    await _context.Animes.AddAsync(anime);
            }

            await _context.SaveChangesAsync();
        }

        private async Task<List<Anime>> DownloadNewAnimeImagesAsync(List<Anime> animes)
        {
            foreach (var anime in animes)
            {
                if (anime.ImgIdFromAnimeGo != null)
                {
                    await using var stream = await _parser.GetAnimeImageAsync(anime.ImgIdFromAnimeGo);
                    var fileInfo = await _imageService.LoadImage(stream);

                    anime.Images = new List<Image>()
                    {
                        new Image()
                        {
                            FileName = fileInfo.Name,
                            Path = fileInfo.Path,
                            Anime = anime
                        }
                    };
                }
            }

            return animes;
        }
    }
}
