using Hng.Application.Features.Jobs.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Jobs.Commands
{
    public class UpdateJobCommand : IRequest<UpdateJobDto>
    {
        public UpdateJobDto UpdateJob { get; set; }
        public Guid JobId { get; set; }
        public UpdateJobCommand(UpdateJobDto updateJob, Guid jobId)
        {
            UpdateJob = updateJob;
            JobId = jobId;
        }
    }
}
