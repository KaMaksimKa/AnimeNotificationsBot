using AnimeNotificationsBot.DAL.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using ParserAnimeGO.Models;
using ParserAnimeGO.Models.ParserModels;

namespace AnimeNotificationsBot.Quartz.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Anime, Anime>()
                .ForMember(d => d.Id, m => m.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
                

            CreateMap<AnimeFullModel, Anime>()
                .ForMember(d => d.Type, m => m.MapFrom(s => s.Type == null ? null : new AnimeType() { Title = s.Type }))
                .ForMember(d => d.Status, m => m.MapFrom(s => s.Status == null ? null : new AnimeStatus() { Title = s.Status }))
                .ForMember(d => d.MpaaRate, m => m.MapFrom(s => s.MpaaRate == null ? null : new MpaaRate() { Title = s.MpaaRate }))
                .ForMember(d => d.Studios, m => m.MapFrom(s => s.Studios.Select(x => new Studio() { Title = x }).ToList()))
                .ForMember(d => d.Genres, m => m.MapFrom(s => s.Genres.Select(x => new Genre() { Title = x }).ToList()))
                .ForMember(d => d.Dubbing, m => m.MapFrom(s => s.Dubbing.Select(x => new Dubbing() { Title = x }).ToList()))
                ;

            CreateMap<AnimeCommentData, AnimeComment>()
                .ForMember(d => d.Id, m => m.MapFrom(s => s.CommentId))
                .ForMember(d => d.CreatedDate,
                    m => m.MapFrom(s =>
                        (DateTimeOffset?)(s.CreatedDate.HasValue ? s.CreatedDate.Value.ToUniversalTime() : null)));

            CreateMap<EpisodeData, Episode>()
                .ForMember(d => d.Released, m => m.MapFrom(s => s.EpisodeReleased ))
                .ForMember(d => d.AnimeId, m => m.Ignore())
                .ForMember(d => d.Number, m => m.MapFrom(s => s.EpisodeNumber))
                .ForMember(d => d.Description, m => m.MapFrom(s => s.EpisodeDescription))
                .ForMember(d => d.Type, m => m.MapFrom(s => s.EpisodeType))
                .ForMember(d => d.Title, m => m.MapFrom(s => s.EpisodeTitle))
                .ForMember(d => d.EpisodeIdFromAnimeGo, m => m.MapFrom(s => s.EpisodeId));

            CreateMap<VideoData, VideoInfo>()
                .ForMember(d => d.VideoPlayerLink, m => m.MapFrom(s => s.VideoPlayerLink))
                .ForMember(d => d.VideoProvider, m => m.MapFrom(s => s.ProviderName != null ? new VideoProvider { Name = s.ProviderName } : null))
                .ForMember(d => d.Dubbing, m => m.MapFrom(s => s.DubbingName != null ? new Dubbing { Title = s.DubbingName } : null));
        }
    }
}
