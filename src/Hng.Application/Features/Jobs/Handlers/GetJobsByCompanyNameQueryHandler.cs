using AutoMapper;
using Hng.Application.Features.Jobs.Dtos;
using Hng.Application.Features.Jobs.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hng.Application.Features.Jobs.Handlers
{
    public class GetJobsByCompanyNameQueryHandler : IRequestHandler<GetJobsByCompanyNameQuery, GetJobByCompanyNameResponseDto>
    {
        private readonly IRepository<Job> _jobRepository;
        private readonly IMapper _mapper;

        public GetJobsByCompanyNameQueryHandler(IRepository<Job> jobRepository, IMapper mapper)
        {
            _jobRepository = jobRepository;
            _mapper = mapper;
        }

        public async Task<GetJobByCompanyNameResponseDto> Handle(GetJobsByCompanyNameQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.CompanyName))
            {
                return new GetJobByCompanyNameResponseDto
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Company name must be provided.",
                    Success = false,
                    Data = null
                };
            }

            var jobs = await _jobRepository.GetAllAsync() ?? new List<Job>();

            jobs = jobs.Where(p => p.Company?.Equals(request.CompanyName, StringComparison.OrdinalIgnoreCase) ?? false)
                .ToList();

            if (!jobs.Any())
            {
                return new GetJobByCompanyNameResponseDto
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "No jobs were found for the specified company.",
                    Success = false,
                    Data = null
                };
            }

            var mappedJobs = _mapper.Map<List<JobDto>>(jobs);

            return new GetJobByCompanyNameResponseDto
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Jobs retrieved successfully.",
                Success = true,
                Data = mappedJobs
            };
        }

    }
}