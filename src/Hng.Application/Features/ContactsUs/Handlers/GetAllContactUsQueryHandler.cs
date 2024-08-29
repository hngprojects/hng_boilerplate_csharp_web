using AutoMapper;
using Hng.Application.Features.ContactsUs.Dtos;
using Hng.Application.Features.ContactsUs.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.ContactsUs.Handlers
{
    public class GetAllContactUsQueryHandler : IRequestHandler<GetAllContactUsQuery, ContactResponse<List<ContactUsResponseDto>>>
    {
        private readonly IRepository<ContactUs> _repository;
        private readonly IMapper _mapper;

        public GetAllContactUsQueryHandler(IRepository<ContactUs> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ContactResponse<List<ContactUsResponseDto>>> Handle(GetAllContactUsQuery request, CancellationToken cancellationToken)
        {
            var contactMessages = await _repository.GetAllAsync();
            if (contactMessages == null)
            {
                return new ContactResponse<List<ContactUsResponseDto>>
                {
                    StatusCode = 400,
                    Message = "Request  Failed",
                    Data = null
                };
            }
            var contactUsResponseDtos = _mapper.Map<List<ContactUsResponseDto>>(contactMessages);
            return new ContactResponse<List<ContactUsResponseDto>>
            {
                StatusCode = 200,
                Message = "Request completed successfully",
                Data = contactUsResponseDtos
            };
        }
    }
}
