using AutoMapper;
using Hng.Application.Features.UserManagement.Dtos;
using Hng.Application.Features.UserManagement.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;

namespace Hng.Application.Features.UserManagement.Handlers
{
    public class GetLoggedInUserDetailsQueryHandler : IRequestHandler<GetLoggedInUserDetailsQuery, UserDto>
    {
        private readonly IAuthenticationService _authService;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public GetLoggedInUserDetailsQueryHandler(IAuthenticationService authService, IRepository<User> _userRepository, IMapper mapper)
        {
            _authService = authService;
            this._userRepository = _userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(GetLoggedInUserDetailsQuery request, CancellationToken cancellationToken)
        {
            var userId = await _authService.GetCurrentUserAsync();

            var user = await _userRepository.GetBySpec(
                u => u.Id == userId,
                u => u.Profile, u => u.Organizations);


            return _mapper.Map<UserDto>(user);

        }
    }
}
