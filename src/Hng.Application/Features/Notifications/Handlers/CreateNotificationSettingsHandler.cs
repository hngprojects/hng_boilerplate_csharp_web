using AutoMapper;
using Hng.Application.Features.Notifications.Commands;
using Hng.Application.Features.Notifications.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Services.Interfaces;
using Hng.Infrastructure.Repository.Interface;
using MediatR;


namespace Hng.Application.Features.Notifications.Handlers
{
    public class CreateNotificationSettingsHandler : IRequestHandler<CreateNotificationSettingsCommand, NotificationSettingsDto>
    {
        private readonly IRepository<NotificationSettings> _notificationSettingsRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;

        public CreateNotificationSettingsHandler(IRepository<NotificationSettings> notificationSettingsRepository, IRepository<User> userRepository, IAuthenticationService authenticationService, IMapper mapper)
        {
            _notificationSettingsRepository = notificationSettingsRepository;
            _userRepository = userRepository;
            _authenticationService = authenticationService;
            _mapper = mapper;
        }

        public async Task<NotificationSettingsDto> Handle(CreateNotificationSettingsCommand request, CancellationToken cancellationToken)
        {
            var userId = await _authenticationService.GetCurrentUserAsync();
            var user = await _userRepository.GetBySpec(u => u.Id == userId);
            if (user != null)
            {
                var existingNotification = await _notificationSettingsRepository.GetBySpec(n => n.UserId == userId);
                if (existingNotification != null)
                {
                    _mapper.Map(request.NotificationBody, existingNotification);
                    existingNotification.UserId = userId;
                    await _notificationSettingsRepository.UpdateAsync(existingNotification);
                    await _notificationSettingsRepository.SaveChanges();
                    return _mapper.Map<NotificationSettingsDto>(existingNotification);
                }
                else
                {
                    var notificationSettings = _mapper.Map<NotificationSettings>(request.NotificationBody);
                    notificationSettings.UserId = userId;
                    await _notificationSettingsRepository.AddAsync(notificationSettings);
                    await _notificationSettingsRepository.SaveChanges();
                    return _mapper.Map<NotificationSettingsDto>(notificationSettings);
                }
            }
            return null;
        }
    }
}


