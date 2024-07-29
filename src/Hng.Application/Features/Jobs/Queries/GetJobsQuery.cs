using Hng.Application.Features.Jobs.Dtos;
using MediatR;

namespace Hng.Application.Features.Jobs.Queries;

public class GetJobsQuery : IRequest<IEnumerable<JobDto>>;