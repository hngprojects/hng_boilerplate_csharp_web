using Hng.Application.Features.HelpCenter.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.HelpCenter.Queries
{
    public class GetHelpCenterTopicsQuery : IRequest<HelpCenterResponseDto<List<HelpCenterTopicResponseDto>>>
    {
    }
}
