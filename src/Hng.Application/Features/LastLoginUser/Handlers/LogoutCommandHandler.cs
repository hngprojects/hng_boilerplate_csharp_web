using AutoMapper;
using Hng.Application.Features.LastLoginUser.Command;
using Hng.Application.Features.LastLoginUser.Dto;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.LastLoginUser.Handlers
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, LastLoginResponseDto<List<LastLoginDto>>>
    {
        private readonly IRepository<LastLogin> _lastLoginRepository;
        private readonly IMapper _mapper;

        public LogoutCommandHandler(IRepository<LastLogin> lastLoginRepository, IMapper mapper)
        {
            _lastLoginRepository = lastLoginRepository;
            _mapper = mapper;
        }

        public async Task<LastLoginResponseDto<List<LastLoginDto>>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var lastLoginList = await _lastLoginRepository.GetAllBySpec(x => x.UserId == request.UserId);

            if (lastLoginList == null)
            {
                return new LastLoginResponseDto<List<LastLoginDto>>
                {
                    StatusCode = 404,
                    Message = "Active login session not found."
                };
            }

            foreach (var lastLogin in lastLoginList)
            {
                lastLogin.LogoutTime = DateTime.UtcNow;
                await _lastLoginRepository.UpdateAsync(lastLogin);
            }


            await _lastLoginRepository.SaveChanges();

            var response = _mapper.Map<List<LastLoginDto>>(lastLoginList);

            return new LastLoginResponseDto<List<LastLoginDto>>
            {
                StatusCode = 200,
                Message = "Logout time updated successfully.",
                Data = response
            };
        }
    }
}
