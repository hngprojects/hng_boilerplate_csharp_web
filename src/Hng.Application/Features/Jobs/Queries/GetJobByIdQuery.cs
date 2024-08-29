using Hng.Application.Features.Jobs.Dtos;
using MediatR;

namespace Hng.Application.Features.Jobs.Queries;

public class GetJobByIdQuery(Guid id) : IRequest<GetJobRequestDto>
{
    public Guid JobId { get; } = id;
}