using Hng.Application.Features.HelpCenter.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.HelpCenter.Queries
{
    public class GetHelpCenterTopicByIdQuery : IRequest<HelpCenterResponseDto<HelpCenterTopicResponseDto>>
    {
        public Guid Id { get; set; }

        public GetHelpCenterTopicByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
