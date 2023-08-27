using AnimeNotificationsBot.Quartz.Configs;
using AnimeNotificationsBot.Quartz.Jobs;
using Microsoft.Extensions.Options;
using Quartz;

namespace AnimeNotificationsBot.Quartz.JobOptions
{
    public class AnimeNotificationOptions : IConfigureOptions<QuartzOptions>
    {

        private readonly QuartzConfig _config;

        public AnimeNotificationOptions(IOptions<QuartzConfig> options)
        {
            _config = options.Value;;
        }
        public void Configure(QuartzOptions options)
        {
            var jobKey = nameof(AnimeNotificationJob);

            options.AddJob<AnimeNotificationJob>(jobBuilder => jobBuilder
                .WithIdentity(jobKey))
                .AddTrigger(trigger => trigger.ForJob(jobKey)
                    .WithCronSchedule(_config.AnimeNotificationCronSchedule));
        }
    }
}
