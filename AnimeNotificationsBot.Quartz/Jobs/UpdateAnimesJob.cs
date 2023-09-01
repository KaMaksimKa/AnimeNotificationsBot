using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ParserAnimeGO;
using ParserAnimeGO.Models;
using Quartz;

namespace AnimeNotificationsBot.Quartz.Jobs
{
    [DisallowConcurrentExecution]
    public class UpdateAnimesJob : IJob
    {
        private readonly ParserAnimeGo _parser;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private List<Genre> _genres = new List<Genre>();
        private List<AnimeType> _types = new List<AnimeType>();
        private List<AnimeStatus> _statuses = new List<AnimeStatus>();
        private List<Studio> _studios = new List<Studio>();
        private List<Dubbing> _dubbing = new List<Dubbing>();
        private List<MpaaRate> _mpaaRates = new List<MpaaRate>();


        public UpdateAnimesJob(ParserAnimeGo parser, DataContext context, IMapper mapper)
        {
            _parser = parser;
            _context = context;
            _mapper = mapper;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            _genres = await _context.Genres.ToListAsync();
            _types = await _context.AnimeTypes.ToListAsync();
            _statuses = await _context.AnimeStatuses.ToListAsync();
            _studios = await _context.Studios.ToListAsync();
            _dubbing = await _context.Dubbing.ToListAsync();
            _mpaaRates = await _context.MpaaRates.ToListAsync();

            List<AnimeFromParser> animesFromParser;
            var numberOfPage = 1;
            do
            {
                animesFromParser = await GetAnimesByPage(numberOfPage);
                var animes = animesFromParser.Select(x => _mapper.Map<Anime>(x)).ToList();
                var preparedAnimes = await PrepareAnimesForAddToContext(animes);
                await AddOrUpdateAnimesInContext(preparedAnimes);
                await _context.SaveChangesAsync();

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
            foreach (var anime in animes)
            {
                #region Подготовка Type

                if (anime.Type != null)
                {
                    var type = _types.FirstOrDefault(x => x.Title == anime.Type.Title);

                    if (type == null)
                    {
                        await _context.AnimeTypes.AddAsync(anime.Type);
                        _types.Add(anime.Type);
                    }
                    else
                        anime.Type = type;
                }

                #endregion

                #region Подготовка MpaaRate

                if (anime.MpaaRate != null)
                {
                    var mpaaRate = _mpaaRates.FirstOrDefault(x => x.Title == anime.MpaaRate.Title);

                    if (mpaaRate == null)
                    {
                        await _context.MpaaRates.AddAsync(anime.MpaaRate);
                        _mpaaRates.Add(anime.MpaaRate);
                    }
                    else
                        anime.MpaaRate = mpaaRate;
                }

                #endregion

                #region Подготовка Status

                if (anime.Status != null)
                {
                    var status = _statuses.FirstOrDefault(x => x.Title == anime.Status.Title);

                    if (status == null)
                    {
                        await _context.AnimeStatuses.AddAsync(anime.Status);
                        _statuses.Add(anime.Status);
                    }
                    else
                        anime.Status = status;
                }

                #endregion

                #region Подготовка Genres

                var genres = new List<Genre>();
                foreach (var genre in anime.Genres)
                {
                    var _genre = _genres.FirstOrDefault(x => x.Title == genre.Title);

                    if (_genre == null)
                    {
                        await _context.Genres.AddAsync(genre);
                        _genres.Add(genre);
                        genres.Add(genre);
                    }
                    else
                        genres.Add(_genre);
                }
                
                anime.Genres = genres;

                #endregion

                #region Подготовка Dubbing

                var dubbing = new List<Dubbing>();
                foreach (var dubbingEntity in anime.Dubbing)
                {
                    var _dubbingEntity = _dubbing.FirstOrDefault(x => x.Title == dubbingEntity.Title);

                    if (_dubbingEntity == null)
                    {
                        await _context.Dubbing.AddAsync(dubbingEntity);
                        _dubbing.Add(dubbingEntity);
                        dubbing.Add(dubbingEntity);
                    }
                    else
                        dubbing.Add(_dubbingEntity);
                }

                anime.Dubbing = dubbing;

                #endregion

                #region Подготовка DubbingFromFirstEpisode

                var dubbingFromFirstEpisode = new List<Dubbing>();
                foreach (var dubbingEntity in anime.DubbingFromFirstEpisode)
                {
                    var _dubbingEntity = _dubbing.FirstOrDefault(x => x.Title == dubbingEntity.Title);

                    if (_dubbingEntity == null)
                    {
                        await _context.Dubbing.AddAsync(dubbingEntity);
                        _dubbing.Add(dubbingEntity);
                        dubbingFromFirstEpisode.Add(dubbingEntity);
                    }
                    else
                        dubbingFromFirstEpisode.Add(_dubbingEntity);
                }

                anime.DubbingFromFirstEpisode = dubbingFromFirstEpisode;

                #endregion

                #region Подготовка Studios

                var studios = new List<Studio>();
                foreach (var studio in anime.Studios)
                {
                    var _studio = _studios.FirstOrDefault(x => x.Title == studio.Title);

                    if (_studio == null)
                    {
                        await _context.Studios.AddAsync(studio);
                        _studios.Add(studio);
                        studios.Add(studio);
                    }
                    else
                        studios.Add(_studio);
                }

                anime.Studios = studios;

                #endregion

            }

            return animes;
        }

        private async Task AddOrUpdateAnimesInContext(List<Anime> animes)
        {
            #region Подготовка Animes

            var _animes = await _context.Animes
                .Include(y => y.Genres)
                .Include(y => y.Dubbing)
                .Include(y => y.DubbingFromFirstEpisode)
                .Include(y => y.Studios)
                .Where(y => animes
                    .Select(z => z.IdFromAnimeGo)
                    .Contains(y.IdFromAnimeGo))
                .ToListAsync();


            foreach (var _anime in _animes)
            {
                _anime.Genres.Clear();
                _anime.Dubbing.Clear();
                _anime.Studios.Clear();
                _anime.DubbingFromFirstEpisode.Clear();
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
        }



    }
}
