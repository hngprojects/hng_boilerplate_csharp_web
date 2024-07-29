using Hng.Application.Features.Notifications.Commands;
using Hng.Application.Features.Notifications.Dtos;
using Hng.Domain.Entities;

namespace Hng.Application.Features.Notifications.Mappers
{
    public class NotificationMapperProfile : AutoMapper.Profile
    {
        public NotificationMapperProfile()
        {
            CreateMap<CreateNotificationCommand, Notification>();
            CreateMap<Notification, NotificationDto>().ReverseMap();
            CreateMap<NotificationDto, Notification>();
        }
    }
}
