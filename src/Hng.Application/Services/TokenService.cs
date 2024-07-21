﻿using AutoMapper;
using Hng.Application.Dto;
using Hng.Application.Interfaces;
using Hng.Domain.Entities;
using Hng.Infrastructure.Context;
using Hng.Infrastructure.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly TimeSpan _tokenExpiry = TimeSpan.FromMinutes(30);
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly MyDBContext _context;

        public TokenService(IUserRepository userRepository, IMapper mapper, MyDBContext context)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _context = context;
        }



        public string GenerateToken()
        {
            using var rng = new RNGCryptoServiceProvider();
            var tokenData = new byte[4];
            rng.GetBytes(tokenData);
            var token = BitConverter.ToUInt32(tokenData, 0) % 1000000; // 6-digit token
            return token.ToString("D6");
        }

        public async Task StoreTokenAsync(string email, string token)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                throw new ArgumentException("User not found", nameof(email));
            }
            var existingToken = await _context.UserTokens
            .FirstOrDefaultAsync(ut => ut.Email == email);
            var userTokenDto = new UserTokenDto
            {
                Email = email,
                Token = token,
                Expiry = DateTime.UtcNow.Add(_tokenExpiry),
                UserId = user.Id
            };
            if (existingToken != null)
            {
                _mapper.Map(userTokenDto, existingToken);
            }
            else
            {
                var userToken = _mapper.Map<UserToken>(userTokenDto);
                _context.UserTokens.Add(userToken);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ValidateTokenAsync(string email, string token)
        {
            var userToken = await _context.UserTokens
                .Where(t => t.Email == email && t.Token == token && t.Expiry > DateTime.UtcNow)
                .FirstOrDefaultAsync();

            return userToken != null;
        }

    }
}
