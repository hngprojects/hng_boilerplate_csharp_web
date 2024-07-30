using AutoMapper;
using Hng.Application.Features.Subscriptions.Commands;
using Hng.Application.Features.Subscriptions.Dtos.Responses;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.Subscriptions.Handlers;

public class ActivateSubscriptionCommandHandler : IRequestHandler<ActivateSubscriptionCommand, SubscriptionDto>
{
    private IRepository<Subscription> _subscriptionRepository;
    private IMapper _mapper;


    public ActivateSubscriptionCommandHandler(IRepository<Subscription> subscriptionRepository, IMapper mapper)
    {
        _subscriptionRepository = subscriptionRepository;
        _mapper = mapper;
    }

    public async Task<SubscriptionDto> Handle(ActivateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var data = await _subscriptionRepository.GetBySpec(
            o => o.Id == request.SubscriptionId);

        if (data != null)
        {
            data.IsActive = true;
            data.UpdatedAt = DateTime.UtcNow;

            await _subscriptionRepository.AddAsync(data);
            await _subscriptionRepository.SaveChanges();

            return _mapper.Map<SubscriptionDto>(data);
        }
        return null;
    }
}