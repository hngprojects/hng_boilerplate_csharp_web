using Hng.Application.Features.ContactsUs.Dtos;
using MediatR;

namespace Hng.Application.Features.ContactsUs.Queries
{
    public class GetAllContactUsQuery : IRequest<ContactResponse<List<ContactUsResponseDto>>>
    {
    }
}
