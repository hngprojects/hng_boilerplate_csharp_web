using Hng.Application.Features.HelpCenter.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.HelpCenter.Command
{
    public class UpdateHelpCenterTopicCommand : IRequest<HelpCenterResponseDto<HelpCenterTopicResponseDto>>
    {
        public Guid Id { get; set; }
        public UpdateHelpCenterTopicRequestDto Request { get; set; }

        public UpdateHelpCenterTopicCommand(Guid id, UpdateHelpCenterTopicRequestDto request)
        {
            Id = id;
            Request = request;
        }
    }
}
