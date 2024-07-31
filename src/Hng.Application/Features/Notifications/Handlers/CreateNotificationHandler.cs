using AutoMapper;
using Hng.Application.Features.Notifications.Commands;
using Hng.Application.Features.Notifications.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;


namespace Hng.Application.Features.Notifications.Handlers
{
    public class CreateNotificationHandler : IRequestHandler<CreateNotificationCommand, NotificationDto>
    {
        private readonly IRepository<Notification> _notificationRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public CreateNotificationHandler(IRepository<Notification> notificationRepository, IRepository<User> userRepository, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<NotificationDto> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(request.LoggedInUserId);
            var user = await _userRepository.GetBySpec(u => u.Id == userId);
            if (user != null)
            {
                var existingNotification = await _notificationRepository.GetBySpec(n => n.UserId == userId);
                if (existingNotification != null)
                {
                    _mapper.Map(request.NotificationBody, existingNotification);
                    existingNotification.UserId = userId;
                    await _notificationRepository.UpdateAsync(existingNotification);
                    await _notificationRepository.SaveChanges();
                    return _mapper.Map<NotificationDto>(existingNotification);
                }
                else
                {
                    var notificationSettings = _mapper.Map<Notification>(request.NotificationBody);
                    notificationSettings.UserId = userId;
                    await _notificationRepository.AddAsync(notificationSettings);
                    await _notificationRepository.SaveChanges();
                    return _mapper.Map<NotificationDto>(notificationSettings);
                }
            }
            return null;
        }
    }
}


