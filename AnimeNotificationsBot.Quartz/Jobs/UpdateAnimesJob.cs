using AnimeNotificationsBot.BLL.Interfaces;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private List<Genre> _genres = new List<Genre>();
        private List<AnimeType> _types = new List<AnimeType>();
        private List<AnimeStatus> _statuses = new List<AnimeStatus>();
        private List<Studio> _studios = new List<Studio>();
        private List<Dubbing> _dubbing = new List<Dubbing>();
        private List<MpaaRate> _mpaaRates = new List<MpaaRate>();


        public UpdateAnimesJob(ParserAnimeGo parser, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _parser = parser;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            _genres = await _unitOfWork.Genres.GetAll();
            _types = await _unitOfWork.AnimesTypes.GetAll();
            _statuses = await _unitOfWork.AnimeStatuses.GetAll();
            _studios = await _unitOfWork.Studios.GetAll();
            _dubbing = await _unitOfWork.Dubbing.GetAll();
            _mpaaRates = await _unitOfWork.MpaaRates.GetAll();

            List<AnimeFromParser> animesFromParser;
            var numberOfPage = 1;
            do
            {
                animesFromParser = await GetAnimesByPage(numberOfPage);
                var animes = animesFromParser.Select(x => _mapper.Map<Anime>(x)).ToList();
                var preparedAnimes = await PrepareAnimesForAddToContext(animes);
                await AddOrUpdateAnimesInContext(preparedAnimes);
                await _unitOfWork.SaveChangesAsync();

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
                        await _unitOfWork.AnimesTypes.AddAsync(anime.Type);
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
                        await _unitOfWork.MpaaRates.AddAsync(anime.MpaaRate);
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
                        await _unitOfWork.AnimeStatuses.AddAsync(anime.Status);
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
                        await _unitOfWork.Genres.AddAsync(genre);
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
                        await _unitOfWork.Dubbing.AddAsync(dubbingEntity);
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
                        await _unitOfWork.Dubbing.AddAsync(dubbingEntity);
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
                        await _unitOfWork.Studios.AddAsync(studio);
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

            var _animes = await _unitOfWork.Animes.GetRangeWhereAsync(async x => await x
                .Include(y => y.Genres)
                .Include(y => y.Dubbing)
                .Include(y => y.DubbingFromFirstEpisode)
                .Include(y => y.Studios)
                .Where(y => animes
                    .Select(z => z.IdFromAnimeGo)
                    .Contains(y.IdFromAnimeGo))
                .ToListAsync());


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
                    _unitOfWork.Animes.Update(animeFromDb);
                }
                else
                    await _unitOfWork.Animes.AddAsync(anime);
            }
        }



    }
}
