using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.Common.Interfaces;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;
using AnimeNotificationsBot.DAL.Migrations;
using AnimeNotificationsBot.Quartz.Models;
using AutoMapper;
using KodikDownloader;
using KodikDownloader.Interfaces;
using Microsoft.EntityFrameworkCore;
using ParserAnimeGO;
using ParserAnimeGO.Interface;
using ParserAnimeGO.Models;
using ParserAnimeGO.Models.ParserModels;
using System.Linq;
using System.Transactions;
using System.Xml.Linq;

namespace AnimeNotificationsBot.Quartz.Services
{
    public class AnimeService
    {
        private readonly ParserAnimeGo _parser;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IAnimeGoUriFactory _animeGoUriFactory;
        private readonly KodikClient _kodikClient;

        public AnimeService(ParserAnimeGo parser, DataContext context, IMapper mapper,
            IAnimeGoUriFactory animeGoUriFactory, KodikClient kodikClient)
        {
            _parser = parser;
            _context = context;
            _mapper = mapper;
            _animeGoUriFactory = animeGoUriFactory;
            _kodikClient = kodikClient;
        }

        public async Task UpdateNotificationsAsync()
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
                .ToList()
                .Distinct()
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
                .Distinct()
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
            var epidsodesSelectorRow = notificationsFromParser
                .Select(x => x.AnimeId + " " + x.EpisodeNumber)
                .ToList();

            var episodes = await _context.Episodes
               .Include(x => x.Anime)
               .Where(x => epidsodesSelectorRow.Contains(x.Anime.AnimeIdFromAnimeGo.ToString() + " " + x.Number.ToString()))
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

            #region Добавление информации о видео в базу

            var episodeIds = episodes.Select(x => x.Id).ToList();
            var updateVideoInfosModels = await _context.Episodes
                .Where(x => episodeIds.Contains(x.Id))
                .Select(x => new UpdateVideoInfoModel
                {
                    AnimeIdFromAnimego = x.Anime.AnimeIdFromAnimeGo,
                    EpisodeIdFromAnimego = x.EpisodeIdFromAnimeGo

                }).ToListAsync();

            var videoInfos = await UpdateVideoInfos(updateVideoInfosModels);

            #endregion

            #region Добавление видео в базу

            await UpdateVideosFromKodik(videoInfos.Select(x => x.VideoPlayerLink).ToList());

            #endregion

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

        public async Task<string?> GetKodikManifestLinkAsync()
        {
            return (await _context.Videos.FirstOrDefaultAsync(x => x.MediaDocumentId == null))?.ManifestLink;
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
                //todo: вынести в парсер
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

        private async Task<List<Episode>> UpdateEpisodeByAnime(List<long> animeIdsFromAnimego)
        {
            var episodes = new List<Episode>();

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
                var episodesMapped = episodesFromParser.Select(x =>
                {
                    var m = _mapper.Map<Episode>(x);
                    m.AnimeId = anime.Id;
                    return m;
                });

                foreach (var episodeMapped in episodesMapped)
                {
                    if (anime.Episodes.FirstOrDefault(x => x.EpisodeIdFromAnimeGo == episodeMapped.EpisodeIdFromAnimeGo) is
                        { } episodeFromDb)
                    {
                        _mapper.Map(episodeMapped, episodeFromDb);
                        episodes.Add(episodeFromDb);
                    }
                    else
                    {
                        await _context.Episodes.AddAsync(episodeMapped);
                        episodes.Add(episodeMapped);
                    }
                }

                
            }

            await _context.SaveChangesAsync();

            return episodes;
        }

        private async Task<List<VideoInfo>> UpdateVideoInfos(List<UpdateVideoInfoModel> models)
        {
            var videoInfos = new List<VideoInfo>();

            var dubbing = await _context.Dubbing.ToListAsync();
            var providers = await _context.VideoProviders.ToListAsync();

            var episodesSelector = models.Select(x => x.EpisodeIdFromAnimego.HasValue
                ? x.AnimeIdFromAnimego.ToString() + " " + x.EpisodeIdFromAnimego.ToString()
                : x.AnimeIdFromAnimego.ToString());

            var episodes = await _context.Episodes
                .Include(x => x.Anime)
                .Include(x => x.VideoInfos)
                    .ThenInclude(x => x.Dubbing)
                .Include(x => x.VideoInfos)
                    .ThenInclude(x => x.VideoProvider)
                .Where(x => episodesSelector.Contains(x.EpisodeIdFromAnimeGo.HasValue
                    ? x.Anime.AnimeIdFromAnimeGo.ToString() + " " + x.EpisodeIdFromAnimeGo.ToString()
                    : x.Anime.AnimeIdFromAnimeGo.ToString()))
                .ToListAsync();

            foreach (var model in models)
            {
                List<VideoInfo> videoInfosMapped;

                var videoDatasFromEpisode = (model.EpisodeIdFromAnimego.HasValue
                    ? (await _parser.GetVideoDatasFromEpisodeAsync(model.EpisodeIdFromAnimego.Value)).Cast<VideoData>()
                    : (await _parser.GetVideoDatasFromFilmAsync(model.AnimeIdFromAnimego)).Cast<VideoData>()).ToList();
                videoInfosMapped = videoDatasFromEpisode.Select(x => _mapper.Map<VideoInfo>(x)).ToList();

                foreach (var videoInfoMapped in videoInfosMapped)
                {
                    videoInfoMapped.Dubbing = videoInfoMapped.Dubbing != null ? await PrepareForAddToContext(videoInfoMapped.Dubbing, dubbing) : null;
                    videoInfoMapped.VideoProvider = videoInfoMapped.VideoProvider != null ? await PrepareForAddToContext(videoInfoMapped.VideoProvider, providers) : null;

                    var episode = episodes.FirstOrDefault(x => x.EpisodeIdFromAnimeGo == model.EpisodeIdFromAnimego 
                        && x.Anime.AnimeIdFromAnimeGo == model.AnimeIdFromAnimego);

                    if (episode != null)
                    {
                        var videoInfoFromDb = episode.VideoInfos
                            .FirstOrDefault(x =>
                                x.Dubbing?.Title == videoInfoMapped.Dubbing?.Title &&
                                x.VideoProvider?.Name == videoInfoMapped.VideoProvider?.Name);

                        if (videoInfoFromDb != null)
                        {
                            _mapper.Map(videoInfoMapped, videoInfoFromDb);
                            videoInfos.Add(videoInfoFromDb);
                        }
                        else
                        {
                            episode.VideoInfos.Add(videoInfoMapped);
                            videoInfos.Add(videoInfoMapped);
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();

            return videoInfos;
        }

        private async Task<List<VideoInfo>> UpdateVideosFromKodik(List<string> shareKodikLinks)
        {
            var videos = new List<Video>();
            var videosInfo = await _context.VideoInfos.Where(x => shareKodikLinks.Contains(x.VideoPlayerLink)).ToListAsync();

            foreach(var shareLink in shareKodikLinks)
            {
                var links = await _kodikClient.GetLinksAsync(shareLink);
                var videoInfo = videosInfo.FirstOrDefault(x => x.VideoPlayerLink == shareLink);

                if (links != null && videoInfo != null)
                {
                    foreach (var link in links.LinksByQuality)
                    {
                        videos.AddRange(link.Value.Select(x => new Video
                        {
                            ManifestLink = x.Link,
                            Quality = link.Key,
                            VideoInfoId = videoInfo.Id
                        }));
                    }
                }
            }
            await _context.Videos.AddRangeAsync(videos);
            await _context.SaveChangesAsync();

            return videosInfo;
        }

        public Task PutKodikManifestLinkAsync(Dictionary<string, int> telegramDocumentIdsByManifestLink)
        {
            throw new NotImplementedException();
        }
    }
}
