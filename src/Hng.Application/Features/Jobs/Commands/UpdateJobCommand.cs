using Hng.Application.Features.Jobs.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Jobs.Commands
{
    public class UpdateJobCommand(UpdateJobDto updateJob, Guid jobId) : IRequest<UpdateJobResponseDto>
    {
        public UpdateJobDto UpdateJob { get; set; } = updateJob;
        public Guid JobId { get; set; } = jobId;

    }
}
