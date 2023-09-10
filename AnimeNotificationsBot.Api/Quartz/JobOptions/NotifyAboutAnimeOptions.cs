using AnimeNotificationsBot.Api.Configs;
using AnimeNotificationsBot.Api.Quartz.Jobs;
using Microsoft.Extensions.Options;
using Quartz;

namespace AnimeNotificationsBot.Api.Quartz.JobOptions
{
    public class NotifyAboutAnimeOptions : IConfigureOptions<QuartzOptions>
    {

        private readonly QuartzConfig _config;

        public NotifyAboutAnimeOptions(IOptions<QuartzConfig> options)
        {
            _config = options.Value;;
        }
        public void Configure(QuartzOptions options)
        {
            var jobKey = nameof(NotifyAboutAnimeJob);

            options.AddJob<NotifyAboutAnimeJob>(jobBuilder => jobBuilder
                .WithIdentity(jobKey))
                .AddTrigger(trigger => trigger.ForJob(jobKey)
                    .WithCronSchedule(_config.NotifyAboutAnimeJobCronSchedule));
        }
    }
}
