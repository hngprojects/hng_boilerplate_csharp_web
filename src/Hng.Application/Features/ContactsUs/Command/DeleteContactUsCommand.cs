using Hng.Application.Features.ContactsUs.Dtos;
using MediatR;

namespace Hng.Application.Features.ContactsUs.Command
{
    public class DeleteContactUsCommand : IRequest<ContactResponse<object>>
    {
        public DeleteContactUsCommand(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; }
    }
}
