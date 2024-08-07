using AutoMapper;
using Hng.Application.Features.Notifications.Commands;
using Hng.Application.Features.Notifications.Dtos;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;

namespace Hng.Application.Features.Notifications.Handlers
{
    public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, NotificationResult>
    {
        private readonly IRepository<NotificationSettings> _notificationSettingsRepository;
        private readonly IRepository<Notification> _notificationRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;

        public CreateNotificationCommandHandler(IRepository<NotificationSettings> notificationSettingsRepository, IRepository<Notification> notificationRepository, IRepository<User> userRepository, IAuthenticationService authenticationService, IMapper mapper)
        {
            _notificationSettingsRepository = notificationSettingsRepository;
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _authenticationService = authenticationService;
            _mapper = mapper;
        }

        public async Task<NotificationResult> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            var userId = await _authenticationService.GetCurrentUserAsync();
            var user = await _userRepository.GetBySpec(u => u.Id == userId);
            var settings = await _notificationSettingsRepository.GetBySpec(u => u.UserId == userId);
            if (user != null)
            {
                if (settings == null || (!settings.EmailNotifications && !settings.ActivityWorkspaceEmail))
                {
                    return new NotificationResult { FailureResponse = new FailureResponseDto<object> { Error = "Not Found", Message = "Notification settings do not allow this action", Data = false } };
                }

                var notification = new Notification
                {
                    UserId = userId,
                    Message = request.Notification.Message,
                };
                await _notificationRepository.AddAsync(notification);
                await _notificationRepository.SaveChanges();
                return new NotificationResult
                {
                    IsSuccess = true,
                    Notification = _mapper.Map<NotificationDto>(notification)
                };
            }
            return new NotificationResult { FailureResponse = new FailureResponseDto<object> { Error = "Not Found", Message = "User not found", Data = false } };
        }
    }
}


