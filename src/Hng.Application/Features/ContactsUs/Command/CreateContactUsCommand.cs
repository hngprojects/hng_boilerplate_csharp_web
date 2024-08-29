using Hng.Application.Features.ContactsUs.Dtos;
using MediatR;

namespace Hng.Application.Features.ContactsUs.Command
{
    public class CreateContactUsCommand : IRequest<ContactResponse<ContactUsResponseDto>>
    {
        public CreateContactUsCommand(ContactUsRequestDto contactUsRequest)
        {
            ContactUsRequest = contactUsRequest;
        }

        public ContactUsRequestDto ContactUsRequest { get; }
    }
}
