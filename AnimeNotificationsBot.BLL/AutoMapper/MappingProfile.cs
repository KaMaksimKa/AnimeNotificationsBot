using AnimeNotificationsBot.BLL.Models.Animes;
using AnimeNotificationsBot.BLL.Models.Dubbing;
using AnimeNotificationsBot.BLL.Models.Notifications;
using AnimeNotificationsBot.DAL.Entities;
using AutoMapper;

namespace AnimeNotificationsBot.BLL.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Dubbing, DubbingModel>();

            CreateMap<Anime, AnimeModel>()
                .ForMember(d => d.IsOngoing, m => m.MapFrom(s => s.Status != null ? s.Status.Title == "Онгоинг" : false));

            CreateMap<AnimeNotification, AnimeNotificationModel>();
        }
    }
}
