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
    public class GetHelpCenterTopicsQueryHandler : IRequestHandler<GetHelpCenterTopicsQuery, HelpCenterResponseDto<List<HelpCenterTopicResponseDto>>>
    {
        private readonly IRepository<HelpCenterTopic> _repository;
        private readonly IMapper _mapper;

        public GetHelpCenterTopicsQueryHandler(IRepository<HelpCenterTopic> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<HelpCenterResponseDto<List<HelpCenterTopicResponseDto>>> Handle(GetHelpCenterTopicsQuery request, CancellationToken cancellationToken)
        {
            var topics = await _repository.GetAllAsync();
            var responseDto = _mapper.Map<List<HelpCenterTopicResponseDto>>(topics);

            return new HelpCenterResponseDto<List<HelpCenterTopicResponseDto>>
            {
                StatusCode = 200,
                Message = "Request completed successfully",
                Data = responseDto
            };
        }
    }

}
