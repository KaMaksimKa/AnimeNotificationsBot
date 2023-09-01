using AnimeNotificationsBot.BLL.Interfaces;
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
        private readonly IUnitOfWork _unitOfWork;

        public AnimeNotificationJob(ParserAnimeGo parser, IUnitOfWork unitOfWork)
        {
            _parser = parser;
            _unitOfWork = unitOfWork;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var notificationsFromParser = (await _parser.GetAnimeNotificationsFromAnimeGoAsync())
                .Where(x => x.IdFromAnimeGo.HasValue && !string.IsNullOrEmpty(x.Dubbing))
                .ToList();

            var animes = await _unitOfWork.Animes.GetRangeWhereAsync(async x => await x
                .Where(y => notificationsFromParser
                    .Select(z => z.IdFromAnimeGo)
                    .Contains(y.IdFromAnimeGo))
                .ToListAsync());

            var dubbing = await _unitOfWork.Dubbing.GetRangeWhereAsync(async x => await x
                .Where(y => notificationsFromParser
                    .Select(z => z.Dubbing)
                    .Contains(y.Title))
                .ToListAsync());

            foreach (var notificationFromParser in notificationsFromParser)
            {
                var anime = animes.FirstOrDefault(x => x.IdFromAnimeGo == notificationFromParser.IdFromAnimeGo);
                var dub = dubbing.FirstOrDefault(x => x.Title == notificationFromParser.Dubbing);
                var notification =await _unitOfWork.AnimeNotifications.GetFirstOrDefaultAsync(async x =>await x
                        .Where(y => notificationFromParser.IdFromAnimeGo == y.Anime.IdFromAnimeGo
                                    && notificationFromParser.Dubbing == y.Dubbing.Title
                                    && notificationFromParser.SerialNumber == y.SerialNumber)
                        .FirstOrDefaultAsync()
                    );

                if (anime != null && dub != null && notification == null)
                {
                    await _unitOfWork.AnimeNotifications.AddAsync(new AnimeNotification()
                    {
                        SerialNumber = notificationFromParser.SerialNumber,
                        Dubbing = dub,
                        Anime = anime,
                        CreatedDate = DateTimeOffset.Now.ToUniversalTime(),
                        IsNotified = false
                    });
                }
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
