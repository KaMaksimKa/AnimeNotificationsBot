using Quartz;

namespace AnimeNotificationsBot.Quartz.Jobs
{
    [DisallowConcurrentExecution]
    public class AnimeNotificationJob : IJob
    {
        private readonly ILogger<AnimeNotificationJob> _logger;

        public AnimeNotificationJob(ILogger<AnimeNotificationJob> logger)
        {
            _logger = logger;
        }
        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation($"log date: {DateTime.Now}");
            return Task.CompletedTask;
        }
    }
}
