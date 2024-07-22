﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using Hng.Application.Dto;
using Hng.Application.Interfaces;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;

namespace Hng.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                return null;
            }

            return _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<IEnumerable<UserDto>> GetUsersByStatusAsync(string org_id, string status, int page, int limit)
        {
            var users = await _userRepository.GetUsersByStatusAsync(org_id, status, page, limit);
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
    }
}
