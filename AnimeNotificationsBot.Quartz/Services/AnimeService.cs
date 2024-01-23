using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.Common.Interfaces;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ParserAnimeGO;
using ParserAnimeGO.Interface;
using ParserAnimeGO.Models;
using ParserAnimeGO.Models.ParserModels;
using System.Transactions;
using System.Xml.Linq;

namespace AnimeNotificationsBot.Quartz.Services
{
    public class AnimeService
    {
        private readonly ParserAnimeGo _parser;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        private readonly IAnimeGoUriFactory _animeGoUriFactory;


        public AnimeService(ParserAnimeGo parser, DataContext context, IMapper mapper, IImageService imageService,
            IAnimeGoUriFactory animeGoUriFactory)
        {
            _parser = parser;
            _context = context;
            _mapper = mapper;
            _imageService = imageService;
            _animeGoUriFactory = animeGoUriFactory;
        }

        public async Task UpdateNotificationsAsync()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                var notificationsFromParser = (await _parser.GetAnimeNotificationsAsync())
                    .Where(x => x.AnimeId.HasValue && !string.IsNullOrEmpty(x.Dubbing))
                    .ToList();

                #region Добавление новых аниме в базу

                var animes = await _context.Animes
                    .Include(x => x.Episodes)
                    .Where(x => notificationsFromParser
                        .Select(y => y.AnimeId)
                        .Contains(x.AnimeIdFromAnimeGo))
                    .ToListAsync();

                var newAnimesIdsFromAnimeGo = notificationsFromParser
                    .Where(x => x.AnimeId.HasValue && animes.All(y => y.AnimeIdFromAnimeGo != x.AnimeId))
                    .Select(x => x.AnimeId!.Value)
                    .ToList();

                if (newAnimesIdsFromAnimeGo.Any())
                {
                    var newAnimesFromParser = await _parser.GetFullAnimesRangeAsync(newAnimesIdsFromAnimeGo);
                    var newAnimes = await SaveAnimeInContext(newAnimesFromParser);

                    animes.AddRange(newAnimes);
                }

                #endregion

                #region Добавление новой озвучки в базу

                var dubbing = await _context.Dubbing
                    .Where(x => notificationsFromParser
                        .Select(y => y.Dubbing)
                        .Contains(x.Title))
                    .ToListAsync();

                var newDubbingTitles = notificationsFromParser
                    .Where(x => x.Dubbing != null && dubbing.All(y => y.Title != x.Dubbing))
                    .Select(x => x.Dubbing!)
                    .ToList();

                if (newDubbingTitles.Any())
                {
                    var newDubbing = newDubbingTitles.Select(x => new Dubbing()
                    {
                        Title = x
                    }).ToList();

                    await _context.Dubbing.AddRangeAsync(newDubbing);

                    await _context.SaveChangesAsync();

                    dubbing.AddRange(newDubbing);
                }

                #endregion

                #region Обновление информации об эпизодах в базу

                await UpdateEpisodeByAnime(notificationsFromParser.Where(x => x.AnimeId.HasValue).Select(x => x.AnimeId!.Value).ToList());

                #endregion

                #region Добавление уведомлений об аниме в базу

                var episodes = await _context.Episodes
                    .Include(x => x.Anime)
                    .Where(x => notificationsFromParser
                        .Select(y => new { AnimeIdFromAnimeGo = y.AnimeId!.Value, EpisodeNumber = y.EpisodeNumber })
                        .ToList()
                        .Contains(new { AnimeIdFromAnimeGo = x.Anime.AnimeIdFromAnimeGo, EpisodeNumber = x.Number }))
                    .ToListAsync();

                foreach (var notificationFromParser in notificationsFromParser)
                {
                    var anime = animes.FirstOrDefault(x => x.AnimeIdFromAnimeGo == notificationFromParser.AnimeId);
                    var dub = dubbing.FirstOrDefault(x => x.Title == notificationFromParser.Dubbing);
                    var episode = episodes.FirstOrDefault(x => x.Anime.AnimeIdFromAnimeGo == notificationFromParser.AnimeId
                        && x.Number == notificationFromParser.EpisodeNumber);

                    var notification = await _context.AnimeNotifications
                            .Where(x => x.Anime.AnimeIdFromAnimeGo == notificationFromParser.AnimeId
                                        && x.Dubbing.Title == notificationFromParser.Dubbing
                                        && (x.Episode.Number == notificationFromParser.EpisodeNumber))
                            .FirstOrDefaultAsync();

                    if (anime != null && dub != null && episode != null && notification == null)
                    {


                        await _context.AnimeNotifications.AddAsync(new AnimeNotification()
                        {
                            Episode = episode,
                            Dubbing = dub,
                            Anime = anime,
                            CreatedDate = DateTimeOffset.Now.ToUniversalTime(),
                            IsNotified = false
                        });
                    }
                }

                await _context.SaveChangesAsync();
                #endregion


                scope.Complete();
            }

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
            List<AnimeFullModel> animesFromParser;
            var numberOfPage = 1;
            do
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    #region Добавление аниме в БД
                    animesFromParser = await _parser.GetFullAnimesByArgsAsync(new AnimesArgs()
                    {
                        PageNumber = numberOfPage
                    });

                    await SaveAnimeInContext(animesFromParser);

                    #endregion

                    #region Добавление новых серий
                    #endregion
                }

                numberOfPage++;

            } while (animesFromParser.Any());


        }

        private async Task<List<Anime>> SaveAnimeInContext(List<AnimeFullModel> animesFromParser)
        {
            var animes = animesFromParser.Select(x => _mapper.Map<Anime>(x)).ToList();
            var preparedAnimes = await PrepareAnimesForAddToContext(animes);
            await AddOrUpdateAnimesInContext(preparedAnimes);
            await _context.SaveChangesAsync();

            return preparedAnimes;
        }

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
                anime.ImgHref = anime.ImgIdFromAnimeGo != null
                    ? _animeGoUriFactory.GetAnimeImage(anime.ImgIdFromAnimeGo).ToString()
                    : null;
                anime.Type = anime.Type == null ? null : await PrepareForAddToContext(anime.Type, types);
                anime.Status = anime.Status == null ? null : await PrepareForAddToContext(anime.Status, statuses);
                anime.MpaaRate = anime.MpaaRate == null ? null : await PrepareForAddToContext(anime.MpaaRate, mpaaRates);
                anime.Genres = await PrepareForAddToContext(anime.Genres, genres);
                anime.Studios = await PrepareForAddToContext(anime.Studios, studios);
                anime.Dubbing = await PrepareForAddToContext(anime.Dubbing, dubbing);
            }

            return animes;
        }

        private async Task<T> PrepareForAddToContext<T>(T entity, List<T> oldEntities) where T : class, IHasUniqueProperty
        {
            var _entity = oldEntities.FirstOrDefault(x => x.UniqueProperty?.Equals(entity.UniqueProperty) == true);

            if (_entity == null)
            {
                await _context.Set<T>().AddAsync(entity);
                oldEntities.Add(entity);
                return entity;
            }
            else
                return _entity;
        }

        private async Task<List<T>> PrepareForAddToContext<T>(List<T> entities, List<T> oldEntities) where T : class, IHasUniqueProperty
        {
            var newEntities = new List<T>();
            foreach (var entity in entities)
            {
                newEntities.Add(await PrepareForAddToContext<T>(entity, oldEntities));
            }

            return newEntities;
        }

        private async Task AddOrUpdateAnimesInContext(List<Anime> animes)
        {
            #region Подготовка Animes

            var _animes = await _context.Animes
                .Include(x => x.Genres)
                .Include(x => x.Dubbing)
                .Include(x => x.Studios)
                .Where(x => animes
                    .Select(y => y.AnimeIdFromAnimeGo)
                    .Contains(x.AnimeIdFromAnimeGo))
                .ToListAsync();


            foreach (var _anime in _animes)
            {
                _anime.Genres.Clear();
                _anime.Dubbing.Clear();
                _anime.Studios.Clear();
            }

            #endregion

            foreach (var anime in animes)
            {
                if (_animes.FirstOrDefault(x => anime.AnimeIdFromAnimeGo == x.AnimeIdFromAnimeGo) is
                    { } animeFromDb)
                {
                    _mapper.Map(anime, animeFromDb);
                }
                else
                    await _context.Animes.AddAsync(anime);
            }
        }

        private async Task UpdateEpisodeByAnime(List<long> animeIdsFromAnimego)
        {
            var animes = await _context.Animes
                .Include(x => x.Episodes)
                .Where(x => animeIdsFromAnimego.Contains(x.AnimeIdFromAnimeGo))
                .ToListAsync();

            foreach (var anime in animes)
            {
                var episodesFromParser = await _parser.GetEpisodesDataAsync(anime.AnimeIdFromAnimeGo);
                if (!episodesFromParser.Any())
                {
                    var videoDatasFromFilm = await _parser.GetVideoDatasFromFilmAsync(anime.AnimeIdFromAnimeGo);
                    if (videoDatasFromFilm.Any())
                    {
                        episodesFromParser.Add(new EpisodeData
                        {
                            AnimeId = anime.AnimeIdFromAnimeGo,
                            EpisodeDescription = null,
                            EpisodeId = null,
                            EpisodeNumber = null,
                            EpisodeReleased = null,
                            EpisodeTitle = null,
                            EpisodeType = null,
                        });
                    }
                }
                var episodesMapped = episodesFromParser.Select(x => _mapper.Map<Episode>(x));

                foreach (var episodeMapped in episodesMapped)
                {
                    if (anime.Episodes.FirstOrDefault(x => x.EpisodeIdFromAnimeGo == episodeMapped.EpisodeIdFromAnimeGo) is
                        { } episodeFromDb)
                    {
                        _mapper.Map(episodeMapped, episodeFromDb);
                    }
                    else
                        anime.Episodes.Add(episodeMapped);
                }
            }

            await _context.SaveChangesAsync();
        }

        private async Task UpdateVideoInfos(List<long>? episodeIdsFromAnimego = null, List<long>? animeIdsFromAnimego = null)
        {
            var dubbing = await _context.Dubbing.ToListAsync();
            var providers = await _context.VideoProviders.ToListAsync();

            if (episodeIdsFromAnimego != null)
            {
                foreach (var episodeIdFromAnimego in episodeIdsFromAnimego)
                {
                    var videoDatasFromEpisode = await _parser.GetVideoDatasFromEpisodeAsync(episodeIdFromAnimego);
                    var videoInfoMapped = videoDatasFromEpisode.Select(x => _mapper.Map<VideoInfo>(x)).ToList();

                    foreach (var videoInfo in videoInfoMapped)
                    {
                        videoInfo.Dubbing = videoInfo.Dubbing != null ? await PrepareForAddToContext(videoInfo.Dubbing, dubbing) : null;
                        videoInfo.VideoProvider = videoInfo.VideoProvider != null ? await PrepareForAddToContext(videoInfo.VideoProvider, providers) : null;

                        if (_animes.FirstOrDefault(x => anime.AnimeIdFromAnimeGo == x.AnimeIdFromAnimeGo) is
                            { } animeFromDb)
                        {
                            _mapper.Map(anime, animeFromDb);
                        }
                        else
                            await _context.Animes.AddAsync(anime);
                    }
                }
            }
        }
    }
}
