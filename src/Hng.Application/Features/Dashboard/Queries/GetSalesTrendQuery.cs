using Hng.Application.Features.Dashboard.Dtos;
using Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Dtos.Responses;
using Hng.Application.Features.Products.Dtos;
using Hng.Application.Shared.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Dashboard.Queries
{
    public class GetSalesTrendQuery : IRequest<PagedListDto<TransactionDto>>
    {
        public GetSalesTrendQuery(SalesTrendQueryParameter parameters)
        {
            productsQueryParameters = parameters;
        }

        public SalesTrendQueryParameter productsQueryParameters { get; set; }
    }
}
