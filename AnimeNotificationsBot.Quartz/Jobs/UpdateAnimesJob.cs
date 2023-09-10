using Quartz;
using AnimeNotificationsBot.Quartz.Services;

namespace AnimeNotificationsBot.Quartz.Jobs
{
    [DisallowConcurrentExecution]
    public class UpdateAnimesJob : IJob
    {
        private readonly AnimeService _animeService;

        public UpdateAnimesJob(AnimeService animeService)
        {
            _animeService = animeService;
        }


        public async Task Execute(IJobExecutionContext context)
        {
            await _animeService.UpdateAllAnimesAsync();
        }
    }
}
