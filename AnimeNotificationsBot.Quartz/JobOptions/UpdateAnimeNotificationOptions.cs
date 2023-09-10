using AnimeNotificationsBot.Quartz.Configs;
using AnimeNotificationsBot.Quartz.Jobs;
using Microsoft.Extensions.Options;
using Quartz;

namespace AnimeNotificationsBot.Quartz.JobOptions
{
    public class UpdateAnimeNotificationOptions : IConfigureOptions<QuartzOptions>
    {

        private readonly QuartzConfig _config;

        public UpdateAnimeNotificationOptions(IOptions<QuartzConfig> options)
        {
            _config = options.Value;;
        }
        public void Configure(QuartzOptions options)
        {
            var jobKey = nameof(UpdateAnimeNotificationJob);

            options.AddJob<UpdateAnimeNotificationJob>(jobBuilder => jobBuilder
                .WithIdentity(jobKey))
                .AddTrigger(trigger => trigger.ForJob(jobKey)
                    .WithCronSchedule(_config.AnimeNotificationCronSchedule));
        }
    }
}
