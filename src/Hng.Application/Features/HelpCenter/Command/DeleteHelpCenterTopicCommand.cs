using Hng.Application.Features.HelpCenter.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.HelpCenter.Command
{
    public class DeleteHelpCenterTopicCommand : IRequest<HelpCenterResponseDto<object>>
    {
        public Guid Id { get; set; }

        public DeleteHelpCenterTopicCommand(Guid id)
        {
            Id = id;
        }
    }
}
