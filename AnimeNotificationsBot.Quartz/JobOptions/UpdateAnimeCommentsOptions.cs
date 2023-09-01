using AnimeNotificationsBot.Quartz.Configs;
using AnimeNotificationsBot.Quartz.Jobs;
using Microsoft.Extensions.Options;
using Quartz;

namespace AnimeNotificationsBot.Quartz.JobOptions
{
    public class UpdateAnimeCommentsOptions: IConfigureOptions<QuartzOptions>
    {
        private readonly QuartzConfig _config;

        public UpdateAnimeCommentsOptions(IOptions<QuartzConfig> options)
        {
            _config = options.Value;
        }

        public void Configure(QuartzOptions options)
        {
            var jobKey = nameof(UpdateAnimeCommentsJob);

            options.AddJob<UpdateAnimeCommentsJob>(jobBuilder => jobBuilder
                    .WithIdentity(jobKey))
                .AddTrigger(trigger => trigger.ForJob(jobKey)
                    .WithCronSchedule(_config.UpdateAnimeCommentsCronSchedule));
        }
    }
}
