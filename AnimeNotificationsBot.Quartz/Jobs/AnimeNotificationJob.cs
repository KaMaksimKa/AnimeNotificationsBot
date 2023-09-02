using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;
using AnimeNotificationsBot.Quartz.Services;
using Microsoft.EntityFrameworkCore;
using ParserAnimeGO;
using Quartz;

namespace AnimeNotificationsBot.Quartz.Jobs
{
    [DisallowConcurrentExecution]
    public class AnimeNotificationJob : IJob
    {
        private readonly AnimeService _animeService;


        public AnimeNotificationJob(AnimeService animeService)
        {
            _animeService = animeService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await _animeService.UpdateNotificationsAsync();
        }
    }
}
