using AnimeNotificationsBot.DAL.Entities;
using AutoMapper;
using ParserAnimeGO.Models;

namespace AnimeNotificationsBot.Quartz.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Anime, Anime>()
                .ForMember(d => d.Id, m => m.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
                

            CreateMap<AnimeFromParser, Anime>()
                .ForMember(d => d.Type, m => m.MapFrom(s => s.Type == null ? null : new AnimeType() { Title = s.Type }))
                .ForMember(d => d.Status, m => m.MapFrom(s => s.Status == null ? null : new AnimeStatus() { Title = s.Status }))
                .ForMember(d => d.MpaaRate, m => m.MapFrom(s => s.MpaaRate == null ? null : new MpaaRate() { Title = s.MpaaRate }))
                .ForMember(d => d.Studios, m => m.MapFrom(s => s.Studios.Select(x => new Studio() { Title = x }).ToList()))
                .ForMember(d => d.Genres, m => m.MapFrom(s => s.Genres.Select(x => new Genre() { Title = x }).ToList()))
                .ForMember(d => d.Dubbing, m => m.MapFrom(s => s.Dubbing.Select(x => new Dubbing() { Title = x }).ToList()))
                .ForMember(d => d.DubbingFromFirstEpisode, m => m.MapFrom(s => s.DubbingFromFirstEpisode.Select(x => new Dubbing() { Title = x }).ToList()))
                ;

            CreateMap<AnimeCommentFromParser, AnimeComment>()
                .ForMember(d => d.Id, m => m.MapFrom(s => s.CommentId))
                .ForMember(d => d.CreatedDate,
                    m => m.MapFrom(s =>
                        (DateTimeOffset?)(s.CreatedDate.HasValue ? s.CreatedDate.Value.ToUniversalTime() : null)));
        }
    }
}
