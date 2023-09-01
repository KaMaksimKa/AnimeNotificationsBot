using AnimeNotificationsBot.Quartz.Configs;
using AnimeNotificationsBot.Quartz.Jobs;
using Microsoft.Extensions.Options;
using Quartz;

namespace AnimeNotificationsBot.Quartz.JobOptions
{
    public class UpdateAnimesOptions: IConfigureOptions<QuartzOptions>
    {
        private readonly QuartzConfig _config;

        public UpdateAnimesOptions(IOptions<QuartzConfig> options)
        {
            _config = options.Value;
        }

        public void Configure(QuartzOptions options)
        {
            var jobKey = nameof(UpdateAnimesJob);

            options.AddJob<UpdateAnimesJob>(jobBuilder => jobBuilder
                    .WithIdentity(jobKey))
                .AddTrigger(trigger => trigger.ForJob(jobKey)
                    .WithCronSchedule(_config.UpdateAnimesCronSchedule));
        }
    }
}
