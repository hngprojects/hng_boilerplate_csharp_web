using AutoMapper;
using Hng.Application.Features.Notifications.Commands;
using Hng.Application.Features.Notifications.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;

namespace Hng.Application.Features.Notifications.Handlers
{
    public class MarkAllCommandHandler : IRequestHandler<MarkAllCommand, List<NotificationDto>>
    {
        private readonly IRepository<Notification> _notificationRepository;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;

        public MarkAllCommandHandler(IRepository<Notification> notificationRepository, IAuthenticationService authenticationService, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _authenticationService = authenticationService;
            _mapper = mapper;
        }

        public async Task<List<NotificationDto>> Handle(MarkAllCommand request, CancellationToken cancellationToken)
        {
            var userId = await _authenticationService.GetCurrentUserAsync();
            var notifications = await _notificationRepository.GetAllBySpec(u => u.UserId == userId);

            if (notifications != null && notifications.Any())
            {
                foreach (var notification in notifications)
                {
                    notification.IsRead = request.IsRead;
                    notification.UpdatedAt = DateTime.UtcNow;
                    await _notificationRepository.UpdateAsync(notification);
                }

                await _notificationRepository.SaveChanges();
                return _mapper.Map<List<NotificationDto>>(notifications);
            }

            return new List<NotificationDto>();
        }
    }
}
