using AutoMapper;
using Hng.Application.Features.HelpCenter.Command;
using Hng.Application.Features.HelpCenter.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.HelpCenter.Handler
{
    public class UpdateHelpCenterTopicCommandHandler : IRequestHandler<UpdateHelpCenterTopicCommand, HelpCenterResponseDto<HelpCenterTopicResponseDto>>
    {
        private readonly IRepository<HelpCenterTopic> _repository;
        private readonly IMapper _mapper;

        public UpdateHelpCenterTopicCommandHandler(IRepository<HelpCenterTopic> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<HelpCenterResponseDto<HelpCenterTopicResponseDto>> Handle(UpdateHelpCenterTopicCommand request, CancellationToken cancellationToken)
        {
            var existingTopic = await _repository.GetAsync(request.Id);
            if (existingTopic == null)
            {
                return new HelpCenterResponseDto<HelpCenterTopicResponseDto>
                {
                    StatusCode = 404,
                    Message = "Topic not found"
                };
            }

            _mapper.Map(request.Request, existingTopic);
            await _repository.UpdateAsync(existingTopic);
            await _repository.SaveChanges();

            var responseDto = _mapper.Map<HelpCenterTopicResponseDto>(existingTopic);
            return new HelpCenterResponseDto<HelpCenterTopicResponseDto>
            {
                StatusCode = 200,
                Message = "Topic updated successfully",
                Data = responseDto
            };
        }
    }

}
