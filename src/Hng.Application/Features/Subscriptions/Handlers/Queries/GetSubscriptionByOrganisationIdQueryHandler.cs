using AutoMapper;
using Hng.Application.Features.Subscriptions.Dtos.Requests;
using Hng.Application.Features.Subscriptions.Dtos.Responses;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Subscriptions.Handlers.Queries
{
    public class GetSubscriptionByOrganisationIdQueryHandler : IRequestHandler<GetSubscriptionByOrganizationIdQuery, SubscriptionDto>
    {
        private readonly IRepository<Subscription> _repository;
        private readonly IMapper _mapper;

        public GetSubscriptionByOrganisationIdQueryHandler(IRepository<Subscription> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<SubscriptionDto> Handle(GetSubscriptionByOrganizationIdQuery request, CancellationToken cancellationToken)
        {
            var subscription = await _repository.GetBySpec(s => s.OrganizationId == request.OrganizationId);
            return subscription != null ? _mapper.Map<SubscriptionDto>(subscription) : null;
        }
    }
}