using AutoMapper;
using Hng.Application.Features.LastLoginUser.Dto;
using Hng.Application.Features.LastLoginUser.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.LastLoginUser.Handlers
{
    public class GetLastLoginQueryHandler : IRequestHandler<GetLastLoginQuery, List<LastLoginDto>>
    {
        private readonly IRepository<LastLogin> _lastLoginRepository;
        private readonly IMapper _mapper;

        public GetLastLoginQueryHandler(IRepository<LastLogin> lastLoginRepository, IMapper mapper)
        {
            _lastLoginRepository = lastLoginRepository;
            _mapper = mapper;
        }

        public async Task<List<LastLoginDto>> Handle(GetLastLoginQuery request, CancellationToken cancellationToken)
        {
            var lastLoginList = await _lastLoginRepository.GetAllBySpec(x => x.UserId == request.UserId);

            if (lastLoginList == null || !lastLoginList.Any())
            {
                throw new FileNotFoundException("Last login not found.");
            }

            var response = _mapper.Map<List<LastLoginDto>>(lastLoginList);

            return response;
        }
    }

}
