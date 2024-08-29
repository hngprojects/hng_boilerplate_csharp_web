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
                StatusCode = 400,
                Message = "Failed to create FAQ",
                Data = null

            };
        }

        await _repository.AddAsync(faq);
        await _repository.SaveChanges();

        return new CreateFaqResponseDto
        {
            StatusCode = 201,
            Message = "FAQ created successfully",
            Data = new FaqData
            {
                Id = faq.Id,
                Question = faq.Question,
                Answer = faq.Answer,
                Category = faq.Category,
                CreatedAt = faq.CreatedAt,
                UpdatedAt = faq.UpdatedAt,
                CreatedBy = faq.CreatedBy
            }
        };
    }
}

