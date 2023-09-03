using AnimeNotificationsBot.Quartz.Services;
using Quartz;

namespace AnimeNotificationsBot.Quartz.Jobs
{
    [DisallowConcurrentExecution]
    public class UpdateAnimeCommentsJob : IJob
    {
        private readonly AnimeService _animeService;

        public UpdateAnimeCommentsJob(AnimeService animeService)
        {
            _animeService = animeService;
        }


        public async Task Execute(IJobExecutionContext context)
        {
            await _animeService.UpdateAllCommentsAsync();
        }
    }
}
