using AutoMapper;
using Hng.Application.Features.Subscriptions.Dtos.Responses;
using Hng.Application.Features.Subscriptions.Queries;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Subscriptions.Handlers.Queries
{
    public class GetSubscriptionsQueryHandler : IRequestHandler<GetSubscriptionsQuery, PagedListDto<SubscriptionDto>>
    {
        private readonly IRepository<Subscription> _subscriptionRepository;
        private readonly IMapper _mapper;

        public GetSubscriptionsQueryHandler(IRepository<Subscription> subscriptionRepository, IMapper mapper)
        {
            _subscriptionRepository = subscriptionRepository;
            _mapper = mapper;
        }

        public async Task<PagedListDto<SubscriptionDto>> Handle(GetSubscriptionsQuery request, CancellationToken cancellationToken)
        {
            var subscriptions = await _subscriptionRepository.GetAllAsync();

            var mappedSubscriptions = _mapper.Map<IEnumerable<SubscriptionDto>>(subscriptions);
            var subscriptionsResult = PagedListDto<SubscriptionDto>.ToPagedList(mappedSubscriptions, request.subscriptionsQueryParameters.PageNumber, request.subscriptionsQueryParameters.PageSize);

            return subscriptionsResult;
        }
    }
}
