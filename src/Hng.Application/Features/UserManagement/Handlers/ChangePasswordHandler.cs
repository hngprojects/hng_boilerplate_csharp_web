using CSharpFunctionalExtensions;
using Hng.Application.Features.UserManagement.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hng.Application.Features.UserManagement.Handlers
{
    public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, Result<ChangePasswordResponse>>
    {
        private readonly IRepository<User> _userRepo;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;

        public ChangePasswordHandler(
            IRepository<User> userRepo,
            IPasswordService passwordService,
            ITokenService tokenService)
        {
            _userRepo = userRepo;
            _passwordService = passwordService;
            _tokenService = tokenService;
        }

        public async Task<Result<ChangePasswordResponse>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var email = _tokenService.GetCurrentUserEmail();

            if (string.IsNullOrWhiteSpace(email))
                return Result.Failure<ChangePasswordResponse>("User does not exist");

            var user = await _userRepo.GetBySpec(u => u.Email == email);

            if (user == null || !_passwordService.IsPasswordEqual(request.OldPassword, user.PasswordSalt, user.Password))
                return Result.Failure<ChangePasswordResponse>("Invalid Password!");

            (user.PasswordSalt, user.Password) = _passwordService.GeneratePasswordSaltAndHash(request.NewPassword);

            await _userRepo.UpdateAsync(user);
            await _userRepo.SaveChanges();

            return Result.Success(new ChangePasswordResponse()
            {
                Message = "successful",
                StatusCode = StatusCodes.Status200OK,
                Data = new ChangePasswordData()
                {
                    Message = "success"
                }
            });
        }
    }
}