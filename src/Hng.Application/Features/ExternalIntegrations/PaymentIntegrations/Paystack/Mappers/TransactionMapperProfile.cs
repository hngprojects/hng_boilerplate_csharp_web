using Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Dtos.Responses;
using Hng.Domain.Entities;

namespace Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Mappers
{
    public class TransactionMapperProfile : AutoMapper.Profile
    {
        public TransactionMapperProfile()
        {
            CreateMap<Transaction, TransactionDto>();
        }
    }
}