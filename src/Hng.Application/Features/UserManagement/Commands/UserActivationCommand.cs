using Hng.Application.Features.UserManagement.Dtos;
using MediatR;

namespace Hng.Application.Features.UserManagement.Commands
{
    public class UserActivationCommand : IRequest<UserActivationResponse>
    {
        public Guid UserId { get; set; }
    }
}
