using AutoMapper;
using Hng.Application.Features.Faq.Commands;
using Hng.Application.Features.Faq.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

public class UpdateFaqCommandHandler : IRequestHandler<UpdateFaqCommand, UpdateFaqResponseDto>
{
    private readonly IRepository<Faq> _repository;
    private readonly IMapper _mapper;

    public UpdateFaqCommandHandler(IRepository<Faq> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<UpdateFaqResponseDto> Handle(UpdateFaqCommand request, CancellationToken cancellationToken)
    {
        var faq = await _repository.GetAsync(request.Id);
        if (faq == null)
        {
            return new UpdateFaqResponseDto
            {
                StatusCode = 404,
                Message = "FAQ not found"
            };
        }

        _mapper.Map(request.FaqRequestDto, faq);
        await _repository.UpdateAsync(faq);

        var responseDto = _mapper.Map<UpdateFaqResponseDto>(faq);
        responseDto.StatusCode = 200;
        responseDto.Message = "FAQ updated successfully";

        return responseDto;
    }
}

