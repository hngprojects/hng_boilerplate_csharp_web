using AutoMapper;
using Hng.Application.Features.ContactsUs.Command;
using Hng.Application.Features.ContactsUs.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.ContactsUs.Handlers
{
    public class CreateContactUsCommandHandler : IRequestHandler<CreateContactUsCommand, ContactResponse<ContactUsResponseDto>>
    {
        private readonly IRepository<ContactUs> _repository;
        private readonly IMapper _mapper;

        public CreateContactUsCommandHandler(IRepository<ContactUs> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ContactResponse<ContactUsResponseDto>> Handle(CreateContactUsCommand request, CancellationToken cancellationToken)
        {
            var contactUsEntity = _mapper.Map<ContactUs>(request.ContactUsRequest);
            if (contactUsEntity == null)
            {
                return new ContactResponse<ContactUsResponseDto>
                {
                    StatusCode = 400,
                    Message = "Failed to create ContactUs due to invalid data.",
                    Data = null
                };
            }
            await _repository.AddAsync(contactUsEntity);
            await _repository.SaveChanges();
            var responseData = _mapper.Map<ContactUsResponseDto>(contactUsEntity);
            return new ContactResponse<ContactUsResponseDto>
            {
                StatusCode = 201,
                Message = "ContactUs created successfully",
                Data = responseData
            };
        }
    }
}
