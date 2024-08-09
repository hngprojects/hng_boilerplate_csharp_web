using AutoMapper;
using Hng.Application.Features.HelpCenter.Dtos;
using Hng.Application.Features.HelpCenter.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.HelpCenter.Handler
{
    public class GetHelpCenterTopicByIdQueryHandler : IRequestHandler<GetHelpCenterTopicByIdQuery, HelpCenterResponseDto<HelpCenterTopicResponseDto>>
    {
        private readonly IRepository<HelpCenterTopic> _repository;
        private readonly IMapper _mapper;

        public GetHelpCenterTopicByIdQueryHandler(IRepository<HelpCenterTopic> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<HelpCenterResponseDto<HelpCenterTopicResponseDto>> Handle(GetHelpCenterTopicByIdQuery request, CancellationToken cancellationToken)
        {
            var topic = await _repository.GetAsync(request.Id);
            if (topic == null)
            {
                return new HelpCenterResponseDto<HelpCenterTopicResponseDto>
                {
                    StatusCode = 404,
                    Message = "Topic not found"
                };
            }

            var responseDto = _mapper.Map<HelpCenterTopicResponseDto>(topic);
            return new HelpCenterResponseDto<HelpCenterTopicResponseDto>
            {
                StatusCode = 200,
                Message = "Request completed successfully",
                Data = responseDto
            };
        }
    }

}
