using AutoMapper;
using Hng.Application.Features.Dashboard.Dtos;
using Hng.Application.Features.Dashboard.Queries;
using Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Dtos.Responses;
using Hng.Application.Features.Products.Dtos;
using Hng.Application.Features.Products.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;

namespace Hng.Application.Features.Dashboard.Handlers
{
    public class GetDashboardQueryHandler : IRequestHandler<GetDashboardQuery, DashboardDto>
    {
        private readonly IRepository<Transaction> _transactionRepository;
        private readonly IRepository<Subscription> _subscriptionRepository;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;

        public GetDashboardQueryHandler(IRepository<Transaction> transactionRepository, IRepository<Subscription> subscriptionRepository, IMapper mapper, IAuthenticationService authenticationService)
        {
            _transactionRepository = transactionRepository;
            _subscriptionRepository = subscriptionRepository;
            _mapper = mapper;
            _authenticationService = authenticationService;
        }

        public async Task<DashboardDto> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
        {
            var userId = await _authenticationService.GetCurrentUserAsync();
            if (userId == Guid.Empty)
            {
                throw new ApplicationException("User ID is not available in the claims.");
            }

            if (request.UserId == Guid.Empty)
            {
                return null;
            }
            var response = new DashboardDto();
            var transactions = await _transactionRepository.GetAllBySpec(x => x.UserId == request.UserId);

            if (transactions.Count() == 0) return null;

            var subscriptions = await _subscriptionRepository.GetAllBySpec(x => x.UserId == request.UserId);

            response.Revenue = transactions.Where(x => x.Status == Domain.Enums.TransactionStatus.Completed).Sum(x => x.Amount);
            response.Sales = transactions.Where(x => x.Status == Domain.Enums.TransactionStatus.Completed).Count();
            response.Subscriptions = subscriptions.Count();
            response.ActiveSubscription = subscriptions.Where(x => x.IsActive).Count();
            response.MonthSales = _mapper.Map<IEnumerable<TransactionDto>>(transactions.Where(x => x.CreatedAt.Month == DateTime.Now.Month && x.CreatedAt.Year == DateTime.Now.Year && x.Status == Domain.Enums.TransactionStatus.Completed));

            return response;
        }
    }
}
