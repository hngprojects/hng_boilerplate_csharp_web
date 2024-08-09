using AutoMapper;
using Hng.Application.Features.HelpCenter.Dtos;
using Hng.Application.Features.HelpCenter.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.HelpCenter.Handler
{
    public class SearchHelpCenterTopicsQueryHandler : IRequestHandler<SearchHelpCenterTopicsQuery, HelpCenterResponseDto<List<HelpCenterTopicResponseDto>>>
    {
        private readonly IRepository<HelpCenterTopic> _repository;
        private readonly IMapper _mapper;

        public SearchHelpCenterTopicsQueryHandler(IRepository<HelpCenterTopic> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<HelpCenterResponseDto<List<HelpCenterTopicResponseDto>>> Handle(SearchHelpCenterTopicsQuery request, CancellationToken cancellationToken)
        {

            Expression<Func<HelpCenterTopic, bool>> predicate = topic =>
                (string.IsNullOrEmpty(request.SearchRequest.Title) || topic.Title.Contains(request.SearchRequest.Title, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(request.SearchRequest.Content) || topic.Content.Contains(request.SearchRequest.Content, StringComparison.OrdinalIgnoreCase));


            var topics = await _repository.GetAllBySpec(predicate);


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
