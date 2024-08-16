using Hng.Application.Features.Dashboard.Dtos;
using Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Dtos.Responses;
using MediatR;

namespace Hng.Application.Features.Dashboard.Queries
{
    public class GetExportDataQuery : IRequest<List<TransactionDto>>
    {
        public GetExportDataQuery(SalesTrendQueryParameter parameters)
        {
            productsQueryParameters = parameters;
        }

        public SalesTrendQueryParameter productsQueryParameters { get; set; }
    }
}
