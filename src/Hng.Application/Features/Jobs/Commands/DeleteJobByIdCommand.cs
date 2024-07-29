using MediatR;

namespace Hng.Application.Features.Jobs.Commands;

public class DeleteJobByIdCommand : IRequest<bool>
{
    public Guid JobId { get; }

    public DeleteJobByIdCommand(Guid jobId)
    {
        JobId = jobId;
    }
}