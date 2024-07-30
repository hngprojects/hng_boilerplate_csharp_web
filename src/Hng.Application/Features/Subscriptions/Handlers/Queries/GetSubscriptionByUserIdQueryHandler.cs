using AutoMapper;
using Hng.Application.Features.Subscriptions.Dtos.Requests;
using Hng.Application.Features.Subscriptions.Dtos.Responses;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Subscriptions.Handlers.Queries
{
    public class GetSubscriptionByUserIdQueryHandler : IRequestHandler<GetSubscriptionByUserIdQuery, SubscriptionDto>
    {
        private readonly IRepository<Subscription> _repository;
        private readonly IMapper _mapper;

        public GetSubscriptionByUserIdQueryHandler(IRepository<Subscription> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<SubscriptionDto> Handle(GetSubscriptionByUserIdQuery request, CancellationToken cancellationToken)
        {
            var subscription = await _repository.GetBySpec(s => s.UserId == request.UserId);
            return subscription != null ? _mapper.Map<SubscriptionDto>(subscription) : null;
        }
    }
}