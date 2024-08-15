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
    public class CreateHelpCenterTopicCommandHandler : IRequestHandler<CreateHelpCenterTopicCommand, HelpCenterResponseDto<HelpCenterTopicResponseDto>>
    {
        private readonly IRepository<HelpCenterTopic> _repository;
        private readonly IMapper _mapper;

        public CreateHelpCenterTopicCommandHandler(IRepository<HelpCenterTopic> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<HelpCenterResponseDto<HelpCenterTopicResponseDto>> Handle(CreateHelpCenterTopicCommand request, CancellationToken cancellationToken)
        {
            var helpCenterTopic = _mapper.Map<HelpCenterTopic>(request);
            if (helpCenterTopic is null)
            {
                return new HelpCenterResponseDto<HelpCenterTopicResponseDto>
                {
                    StatusCode = 400,
                    Message = "Help Center Topic not created successfully",

                };
            }
            await _repository.AddAsync(helpCenterTopic);
            await _repository.SaveChanges();

            var responseDto = _mapper.Map<HelpCenterTopicResponseDto>(helpCenterTopic);
            return new HelpCenterResponseDto<HelpCenterTopicResponseDto>
            {
                StatusCode = 201,
                Message = "Help Center Topic created successfully",
                Data = responseDto
            };
        }
    }
}
