using Hng.Application.Features.Notifications.Commands;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;

namespace Hng.Application.Features.Notifications.Handlers
{
    public class DeleteNotificationByIdCommandHandler : IRequestHandler<DeleteNotificationByIdCommand, bool>
    {
        private readonly IRepository<Notification> _notificationRepository;
        private readonly IAuthenticationService _authenticationService;

        public DeleteNotificationByIdCommandHandler(IRepository<Notification> notificationRepository, IAuthenticationService authenticationService)
        {
            _notificationRepository = notificationRepository;
            _authenticationService = authenticationService;
        }

        public async Task<bool> Handle(DeleteNotificationByIdCommand request, CancellationToken cancellationToken)
        {
            var userId = await _authenticationService.GetCurrentUserAsync();
            var notification = await _notificationRepository.GetBySpec(n => n.Id == request.NotificationId && n.UserId == userId);

            if (notification != null)
            {
                await _notificationRepository.DeleteAsync(notification);
                await _notificationRepository.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
