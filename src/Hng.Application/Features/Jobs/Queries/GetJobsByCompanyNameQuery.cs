using Hng.Application.Features.Jobs.Dtos;
using MediatR;

namespace Hng.Application.Features.Jobs.Queries;

public class GetJobsByCompanyNameQuery : IRequest<GetJobByCompanyNameResponseDto>
{
    public string CompanyName { get; set; }

    public GetJobsByCompanyNameQuery(string companyName)
    {
        CompanyName = companyName;
    }
}