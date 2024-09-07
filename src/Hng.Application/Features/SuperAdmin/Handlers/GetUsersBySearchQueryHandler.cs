using AutoMapper;
using Hng.Application.Features.SuperAdmin.Dto;
using Hng.Application.Features.SuperAdmin.Queries;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.SuperAdmin.Handlers
{
    public class GetUsersBySearchQueryHandler : IRequestHandler<GetUsersBySearchQuery, PagedListDto<UserSuperDto>>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public GetUsersBySearchQueryHandler(IRepository<User> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<PagedListDto<UserSuperDto>> Handle(GetUsersBySearchQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync();
            users = users.Where(v => request.usersQueryParameters.Email == null || v.Email.ToLower().Equals(request.usersQueryParameters.Email.ToLower())).ToList();
            users = users.Where(v => request.usersQueryParameters.Firstname == null || v.FirstName.ToLower().Equals(request.usersQueryParameters.Firstname.ToLower())).ToList();
            users = users.Where(v => request.usersQueryParameters.Lastname == null || v.LastName.ToLower().Equals(request.usersQueryParameters.Lastname.ToLower())).ToList();

            var mappedusers = _mapper.Map<IEnumerable<UserSuperDto>>(users);
            var userSearchResult = PagedListDto<UserSuperDto>.ToPagedList(mappedusers, request.usersQueryParameters.Offset, request.usersQueryParameters.Limit);

            return userSearchResult;
        }
    }
}
