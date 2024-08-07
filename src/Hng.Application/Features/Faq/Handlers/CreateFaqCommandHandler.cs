using AutoMapper;
using Hng.Application.Features.Faq.Commands;
using Hng.Application.Features.Faq.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

public class CreateFaqCommandHandler : IRequestHandler<CreateFaqCommand, CreateFaqResponseDto>
{
    private readonly IRepository<Faq> _repository;
    private readonly IMapper _mapper;

    public CreateFaqCommandHandler(IRepository<Faq> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CreateFaqResponseDto> Handle(CreateFaqCommand request, CancellationToken cancellationToken)
    {
        var faq = _mapper.Map<Faq>(request.FaqRequestDto);
        if (faq is null)
        {
            return new CreateFaqResponseDto
            {
                StatusCode = 500,
                Error = "FAQ not Created",
                Message = "Failed to create FAQ"
            };
        }

        await _repository.AddAsync(faq);

        var responseDto = _mapper.Map<CreateFaqResponseDto>(faq);
        responseDto.StatusCode = 201;
        responseDto.Message = "FAQ created successfully";

        return responseDto;
    }
}

