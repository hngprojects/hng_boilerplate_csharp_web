using Hng.Application.Features.Faq.Commands;
using Hng.Application.Features.Faq.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

public class DeleteFaqCommandHandler : IRequestHandler<DeleteFaqCommand, DeleteFaqResponseDto>
{
    private readonly IRepository<Faq> _repository;

    public DeleteFaqCommandHandler(IRepository<Faq> repository)
    {
        _repository = repository;
    }

    public async Task<DeleteFaqResponseDto> Handle(DeleteFaqCommand request, CancellationToken cancellationToken)
    {
        var faq = await _repository.GetAsync(request.Id);
        if (faq == null)
        {
            return new DeleteFaqResponseDto
            {
                StatusCode = 404,
                Message = "FAQ not found"
            };
        }

        await _repository.DeleteAsync(faq);
        await _repository.SaveChanges();

        return new DeleteFaqResponseDto
        {
            StatusCode = 200,
            Message = "FAQ deleted successfully"
        };
    }
}

