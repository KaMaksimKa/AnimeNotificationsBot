using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ParserAnimeGO;
using Quartz;

namespace AnimeNotificationsBot.Quartz.Jobs
{
    [DisallowConcurrentExecution]
    public class UpdateAnimeCommentsJob : IJob
    {
        private readonly ParserAnimeGo _parser;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UpdateAnimeCommentsJob(ParserAnimeGo parser, DataContext context, IMapper mapper)
        {
            _parser = parser;
            _context = context;
            _mapper = mapper;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            int numberOfPage = 55;//старуем отсюда пока что
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

                    var commentsFromParser = await _parser.GetAllCommentsFromAnime(anime.IdForComments.Value,200);
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
    }
}
