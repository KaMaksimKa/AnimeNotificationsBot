using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using ParserAnimeGO;
using Quartz;

namespace AnimeNotificationsBot.Quartz.Jobs
{
    [DisallowConcurrentExecution]
    public class AnimeNotificationJob : IJob
    {
        private readonly ParserAnimeGo _parser;
        private readonly DataContext _context;


        public AnimeNotificationJob(ParserAnimeGo parser, DataContext context)
        {
            _parser = parser;
            _context = context;
        }
        public async Task Execute(IJobExecutionContext context)
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
                var notification =await _context.AnimeNotifications
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
    }
}
