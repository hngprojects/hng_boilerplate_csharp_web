using Hng.Application.Features.Jobs.Commands;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Jobs.Handlers;

public class DeleteJobByIdCommandHandler : IRequestHandler<DeleteJobByIdCommand, bool>
{
    private readonly IRepository<Job> _jobRepository;

    public DeleteJobByIdCommandHandler(IRepository<Job> jobRepository)
    {
        _jobRepository = jobRepository;
    }

    public async Task<bool> Handle(DeleteJobByIdCommand request, CancellationToken cancellationToken)
    {
        var job = await _jobRepository.GetBySpec(j => j.Id == request.JobId);
        if (job == null)
        {
            return false;
        }
        await _jobRepository.DeleteAsync(job);
        await _jobRepository.SaveChanges();
        return true;
    }
}