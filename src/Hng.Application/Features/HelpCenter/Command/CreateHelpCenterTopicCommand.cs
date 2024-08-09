using Hng.Application.Features.HelpCenter.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.HelpCenter.Command
{
    public class CreateHelpCenterTopicCommand : IRequest<HelpCenterResponseDto<HelpCenterTopicResponseDto>>
    {
        public CreateHelpCenterTopicCommand(CreateHelpCenterTopicRequestDto request)
        {
            Request = request;
        }

        public CreateHelpCenterTopicRequestDto Request { get; }
    }
}
