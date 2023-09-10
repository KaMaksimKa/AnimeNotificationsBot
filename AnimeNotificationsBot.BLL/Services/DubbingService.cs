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
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AnimeNotificationsBot.BLL.Services
{
    public class DubbingService : IDubbingService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public DubbingService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
                .Select(x => _mapper.Map<DubbingModel>(x)).ToList();
        }
    }
}
