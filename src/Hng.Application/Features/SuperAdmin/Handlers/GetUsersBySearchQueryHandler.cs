using AutoMapper;
using Hng.Application.Features.SuperAdmin.Dto;
using Hng.Application.Features.SuperAdmin.Queries;
using Hng.Application.Shared.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;

namespace Hng.Application.Features.SuperAdmin.Handlers
{
    public class GetUsersBySearchQueryHandler : IRequestHandler<GetUsersBySearchQuery, PagedListDto<UserDto>>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public GetUsersBySearchQueryHandler(IRepository<User> userService, IMapper mapper)
        {
            _userRepository = userService;
            _mapper = mapper;
        }

        public async Task<PagedListDto<UserDto>> Handle(GetUsersBySearchQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllBySpec(v => EF.Functions.Like(v.FirstName, request.usersQueryParameters.Firstname), v => EF.Functions.Like(v.LastName, request.usersQueryParameters.Lastname), v => EF.Functions.Like(v.Email, request.usersQueryParameters.Email));

            var mappedusers = _mapper.Map<IEnumerable<UserDto>>(users);
            var userSearchResult = PagedListDto<UserDto>.ToPagedList(mappedusers, request.usersQueryParameters.PageNumber, request.usersQueryParameters.PageSize);

            return userSearchResult;
        }
    }
}
