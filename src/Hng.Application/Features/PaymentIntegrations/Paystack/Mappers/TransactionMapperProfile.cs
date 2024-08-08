using Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Responses;
using Hng.Domain.Entities;

namespace Hng.Application.Features.PaymentIntegrations.Paystack.Mappers
{
    public class TransactionMapperProfile : AutoMapper.Profile
    {
        public TransactionMapperProfile()
        {
            CreateMap<Transaction, TransactionDto>();
        }
    }
}