using AutoMapper;
using Hng.Application.Features.Jobs.Commands;
using Hng.Application.Features.Jobs.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Mysqlx.Notice.Warning.Types;

namespace Hng.Application.Features.Jobs.Handlers
{
    public class UpdateJobCommandHandler : IRequestHandler<UpdateJobCommand, UpdateJobResponseDto>
    {

        private readonly IRepository<Job> _jobRepository;
        private readonly IMapper _mapper;

        public UpdateJobCommandHandler(IRepository<Job> jobRepository, IMapper mapper)
        {
            _jobRepository = jobRepository;
            _mapper = mapper;
        }

        public async Task<UpdateJobResponseDto> Handle(UpdateJobCommand request, CancellationToken cancellationToken)
        {
            // Get the user from the database
            var job = await _jobRepository.GetAsync(request.JobId);

            // Return a bad response if the user is not found
            if (job == null)
            {
                return new UpdateJobResponseDto()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Job not found",
                    Success = false
                };
            }

            // Update existing job
            _mapper.Map(request.UpdateJob, job);

            // Update and save the job in the database
            await _jobRepository.UpdateAsync(job);
            await _jobRepository.SaveChanges();

            // Return a success response
            return new UpdateJobResponseDto()
            {
                Message = "Job updated successfully",
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = job
            };
        }
    }
}
