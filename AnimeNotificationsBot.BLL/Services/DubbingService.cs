using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.BLL.Models.Dubbing;
using AnimeNotificationsBot.Common.Exceptions;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace AnimeNotificationsBot.BLL.Services
{
    public class DubbingService : IDubbingService
    {
        private readonly DataContext _context;

        public DubbingService(DataContext context)
        {
            _context = context;
        }
        public async Task<List<DubbingModel>> GetDubbingByAnimeIdAsync(long animeId)
        {
            var anime = await _context.Animes
                .Include(x => x.DubbingFromFirstEpisode)
                .FirstOrDefaultAsync(x => x.Id == animeId);

            if (anime == null)
            {
                throw new NotFoundEntityException()
                {
                    EntityName = nameof(Anime),
                    PropertyName = nameof(anime.Id),
                    PropertyValue = animeId
                };
            }

            return anime.DubbingFromFirstEpisode
                .Select(x => new DubbingModel()
                {
                    Id = x.Id,
                    Title = x.Title,
                }).ToList();
        }
    }
}
