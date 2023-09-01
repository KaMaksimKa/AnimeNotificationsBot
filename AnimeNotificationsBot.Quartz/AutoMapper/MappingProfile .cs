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
                .ForMember(d => d.Id, m => m.Ignore());

            CreateMap<AnimeFromParser, Anime>()
                .ForMember(d => d.IdFromAnimeGo, m => m.MapFrom(s => s.IdFromAnimeGo))
                .ForMember(d => d.TitleEn, m => m.MapFrom(s => s.TitleEn))
                .ForMember(d => d.TitleRu, m => m.MapFrom(s => s.TitleRu))
                .ForMember(d => d.Year, m => m.MapFrom(s => s.Year))
                .ForMember(d => d.Description, m => m.MapFrom(s => s.Description))
                .ForMember(d => d.Rate, m => m.MapFrom(s => s.Rate))
                .ForMember(d => d.CountEpisode, m => m.MapFrom(s => s.CountEpisode))
                .ForMember(d => d.Planned, m => m.MapFrom(s => s.Planned))
                .ForMember(d => d.Completed, m => m.MapFrom(s => s.Completed))
                .ForMember(d => d.Watching, m => m.MapFrom(s => s.Watching))
                .ForMember(d => d.Dropped, m => m.MapFrom(s => s.Dropped))
                .ForMember(d => d.OnHold, m => m.MapFrom(s => s.OnHold))
                .ForMember(d => d.Href, m => m.MapFrom(s => s.Href))
                .ForMember(d => d.ImgHref, m => m.MapFrom(s => s.ImgHref))
                .ForMember(d => d.NextEpisode, m => m.MapFrom(s => s.NextEpisode))
                .ForMember(d => d.Duration, m => m.MapFrom(s => s.Duration))
                .ForMember(d => d.IdForComments, m => m.MapFrom(s => s.IdForComments))
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
                .ForMember(d => d.AuthorName, m => m.MapFrom(s => s.AuthorName))
                .ForMember(d => d.Comment, m => m.MapFrom(s => s.Comment))
                .ForMember(d => d.CreatedDate, m => m.MapFrom(s => (DateTimeOffset?)(s.CreatedDate.HasValue ? s.CreatedDate.Value.ToUniversalTime() : null)))
                .ForMember(d => d.ParentCommentId, m => m.MapFrom(s => s.ParentCommentId))
                .ForMember(d => d.Score, m => m.MapFrom(s => s.Score));
        }
    }
}
