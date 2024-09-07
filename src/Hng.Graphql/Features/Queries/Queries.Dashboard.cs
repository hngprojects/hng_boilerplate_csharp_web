using Hng.Application.Features.Dashboard.Dtos;
using Hng.Application.Features.Dashboard.Queries;
using Hng.Application.Features.ExternalIntegrations.PaymentIntegrations.Paystack.Dtos.Responses;
using Hng.Application.Shared.Dtos;
using HotChocolate.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Graphql
{
    public partial class Queries
    {
        [Authorize]
        public async Task<DashboardDto> GetUserProduct(Guid userId, [FromServices] IMediator mediator)
        {
            var query = new GetDashboardQuery(userId);
            return await mediator.Send(query);
        }

        [Authorize]
        public async Task<PagedListDto<TransactionDto>> GetSalesTrend(SalesTrendQueryParameter parameter, [FromServices] IMediator mediator)
        {
            var query = new GetSalesTrendQuery(parameter);
            return await mediator.Send(query);
        }

        [Authorize]
        public async Task<NavigationDataDto> GetNavigationData([FromServices] IMediator mediator)
        {
            var query = new GetNavigationDataQuery();
            return await mediator.Send(query);
        }

        [Authorize]
        public async Task<List<TransactionDto>> GetExportData(SalesTrendQueryParameter parameter, [FromServices] IMediator mediator)
        {
            var query = new GetExportDataQuery(parameter);
            return await mediator.Send(query);
        }
    }
}
