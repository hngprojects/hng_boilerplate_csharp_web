using MediatR;

namespace Hng.Application.Features.Jobs.Commands;

public class DeleteJobCommand : IRequest<bool>
{
    public Guid JobId { get; }

    public DeleteJobCommand(Guid jobId)
    {
        JobId = jobId;
    }
}