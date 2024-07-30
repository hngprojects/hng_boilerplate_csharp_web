using Hng.Application.Features.Notifications.Dtos;
using Hng.Domain.Entities;

namespace Hng.Application.Features.Notifications.Mappers
{
    public class NotificationMapperProfile : AutoMapper.Profile
    {
        public NotificationMapperProfile()
        {
            CreateMap<CreateNotificationDto, Notification>();
            CreateMap<Notification, NotificationDto>();
        }
    }
}
