using Hng.Application.Features.Jobs.Dtos;
using MediatR;

namespace Hng.Application.Features.Jobs.Commands
{
    public class CreateJobCommand(CreateJobDto createJobDto) : IRequest<JobDto>
    {
        public CreateJobDto JobBody { get; } = createJobDto;
    }
}