using Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Dtos.Responses;
using Hng.Domain.Entities;

namespace Hng.Application.Features.Dashboard.Mappers
{
    public class DashboardMapperProfile : AutoMapper.Profile
    {
        public DashboardMapperProfile()
        {
            CreateMap<Transaction, TransactionDto>().ReverseMap();
        }
    }
}
