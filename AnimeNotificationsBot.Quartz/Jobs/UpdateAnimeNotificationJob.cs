using AnimeNotificationsBot.Quartz.Services;
using Quartz;

namespace AnimeNotificationsBot.Quartz.Jobs
{
    [DisallowConcurrentExecution]
    public class UpdateAnimeNotificationJob : IJob
    {
        private readonly AnimeService _animeService;


        public UpdateAnimeNotificationJob(AnimeService animeService)
        {
            _animeService = animeService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await _animeService.UpdateNotificationsAsync();
        }
    }
}
