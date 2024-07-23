using AutoMapper;
using Hng.Application.Features.UserManagement.Commands;
using Hng.Application.Features.UserManagement.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.UserManagement.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private IRepository<User> _userRepo;
        private IMapper _mapper;

        public CreateUserCommandHandler(IRepository<User> userRepository, IMapper mapper)
        {
            _userRepo = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request.UserBody);
            await _userRepo.AddAsync(user);
            await _userRepo.SaveChanges();
            return _mapper.Map<UserDto>(user);
        }
    }
}