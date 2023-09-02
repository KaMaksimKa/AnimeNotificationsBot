using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.Common.Interfaces;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ParserAnimeGO;
using ParserAnimeGO.Models;
using Quartz;
using System;
using System.Xml.Linq;
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
