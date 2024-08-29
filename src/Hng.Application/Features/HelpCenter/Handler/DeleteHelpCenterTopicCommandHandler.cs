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
    public class DeleteHelpCenterTopicCommandHandler : IRequestHandler<DeleteHelpCenterTopicCommand, HelpCenterResponseDto<object>>
    {
        private readonly IRepository<HelpCenterTopic> _repository;

        public DeleteHelpCenterTopicCommandHandler(IRepository<HelpCenterTopic> repository)
        {
            _repository = repository;
        }

        public async Task<HelpCenterResponseDto<object>> Handle(DeleteHelpCenterTopicCommand request, CancellationToken cancellationToken)
        {
            var topic = await _repository.GetAsync(request.Id);
            if (topic == null)
            {
                return new HelpCenterResponseDto<object>
                {
                    StatusCode = 404,
                    Message = "Topic not found"
                };
            }

            await _repository.DeleteAsync(topic);
            await _repository.SaveChanges();
            return new HelpCenterResponseDto<object>
            {
                StatusCode = 200,
                Message = "Topic deleted successfully"
            };
        }
    }

}
