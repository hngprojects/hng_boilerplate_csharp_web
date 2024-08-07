using AutoMapper;
using Hng.Application.Features.Faq.Dtos;
using Hng.Application.Features.Faq.Queries;
using Hng.Infrastructure.Repository.Interface;
using Hng.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Faq.Handlers
{
    public class GetAllFaqsQueryHandler : IRequestHandler<GetAllFaqsQuery, List<FaqResponseDto>>
    {
        private readonly IRepository<Domain.Entities.Faq> _repository;
        private readonly IMapper _mapper;

        public GetAllFaqsQueryHandler(IRepository<Domain.Entities.Faq> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<FaqResponseDto>> Handle(GetAllFaqsQuery request, CancellationToken cancellationToken)
        {
            var faqs = await _repository.GetAllAsync();
            return _mapper.Map<List<FaqResponseDto>>(faqs);
        }
    }
}
