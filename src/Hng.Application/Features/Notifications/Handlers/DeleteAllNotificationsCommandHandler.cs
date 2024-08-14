using Hng.Application.Features.Notifications.Commands;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;

namespace Hng.Application.Features.Notifications.Handlers
{
    public class DeleteAllNotificationsCommandHandler : IRequestHandler<DeleteAllNotificationsCommand, bool>
    {
        private readonly IRepository<Notification> _notificationRepository;
        private readonly IAuthenticationService _authenticationService;

        public DeleteAllNotificationsCommandHandler(IRepository<Notification> notificationRepository, IAuthenticationService authenticationService)
        {
            _notificationRepository = notificationRepository;
            _authenticationService = authenticationService;
        }

        public async Task<bool> Handle(DeleteAllNotificationsCommand request, CancellationToken cancellationToken)
        {
            var userId = await _authenticationService.GetCurrentUserAsync();
            var notifications = await _notificationRepository.GetAllBySpec(n => n.UserId == userId);

            if (notifications != null && notifications.Any())
            {
                foreach (var notification in notifications)
                {
                    await _notificationRepository.DeleteAsync(notification);
                }

                await _notificationRepository.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
