using AutoMapper;
using Hng.Application.Features.Subscriptions.Dtos.Requests;
using Hng.Application.Features.Subscriptions.Dtos.Responses;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Subscriptions.Handlers.Queries
{
    public class GetSubscriptionByOrganisationIdQueryHandler : IRequestHandler<GetSubscriptionByOrganizationIdQuery, GetSubscriptionByOrganizationIdResponse>
    {
        private readonly IRepository<Subscription> _repository;
        private readonly IMapper _mapper;

        public GetSubscriptionByOrganisationIdQueryHandler(IRepository<Subscription> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<GetSubscriptionByOrganizationIdResponse> Handle(GetSubscriptionByOrganizationIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var subscriptions = await _repository.GetAllBySpec(s => s.OrganizationId == request.OrganizationId);

                if (subscriptions == null || !subscriptions.Any())
                {
                    return new GetSubscriptionByOrganizationIdResponse
                    {
                        Status = false,
                        Error = "Subscriptions not found."
                    };
                }

                var subscriptionDtos = _mapper.Map<List<SubscriptionDto>>(subscriptions);

                return new GetSubscriptionByOrganizationIdResponse
                {
                    Status = true,
                    Subscriptions = subscriptionDtos
                };
            }
            catch (Exception ex)
            {
                // Log the exception if you have a logging framework in place
                return new GetSubscriptionByOrganizationIdResponse
                {
                    Status = false,
                    Error = "An unexpected error occurred."
                };
            }
        }
    }
}
    

