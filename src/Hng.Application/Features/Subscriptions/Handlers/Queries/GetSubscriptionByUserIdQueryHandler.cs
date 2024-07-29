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
    public class GetSubscriptionByUserIdQueryHandler : IRequestHandler<GetSubscriptionByUserIdQuery, GetSubscriptionByUserIdResponse>
    {
        private readonly IRepository<Subscription> _repository;
        private readonly IMapper _mapper;

        public GetSubscriptionByUserIdQueryHandler(IRepository<Subscription> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetSubscriptionByUserIdResponse> Handle(GetSubscriptionByUserIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var subscription = await _repository.GetAllBySpec(s => s.UserId == request.UserId);

                if (subscription == null)
                {
                    return new GetSubscriptionByUserIdResponse
                    {
                        Status = false,
                        Error = "Subscription not found."
                    };
                }

                var response = _mapper.Map<GetSubscriptionByUserIdResponse>(subscription);
                response.Status = true;

                return response;
            }
            catch (Exception ex)
            {
                return new GetSubscriptionByUserIdResponse
                {
                    Status = false,
                    Error = "An unexpected error occurred."
                };
            }
        }
    }
}
