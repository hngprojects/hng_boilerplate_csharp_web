using CSharpFunctionalExtensions;
using Hng.Application.Features.UserManagement.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hng.Application.Features.UserManagement.Handlers
{
    public record PasswordResetHandler : IRequestHandler<PasswordResetDto, Result<PasswordResetResponse>>
    {
        private readonly IRepository<User> _userRepo;
        private readonly IPasswordService _passwordService;

        public PasswordResetHandler(
            IRepository<User> userRepo,
            IPasswordService passwordService)
        {
            _userRepo = userRepo;
            _passwordService = passwordService;
        }

        public async Task<Result<PasswordResetResponse>> Handle(PasswordResetDto request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetBySpec(u => u.Id.ToString() == request.UserId && u.PasswordResetToken == request.Code);

            if (user == null)
                return Result.Failure<PasswordResetResponse>("This password reset link has been used!");

            if ((DateTime.UtcNow - user.PasswordResetTokenTime.GetValueOrDefault()).Hours >= 24)
                return Result.Failure<PasswordResetResponse>("Reset Link has expired!");

            (user.PasswordSalt, user.Password) = _passwordService.GeneratePasswordSaltAndHash(request.NewPassword);
            user.PasswordResetToken = null;

            await _userRepo.UpdateAsync(user);
            await _userRepo.SaveChanges();

            return Result.Success(new PasswordResetResponse()
            {
                Message = "successful",
                StatusCode = StatusCodes.Status200OK,
                Data = new PasswordResetData()
                {
                    Message = "success"
                }
            });
        }
    }
}