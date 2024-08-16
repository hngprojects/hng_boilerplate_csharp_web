using CSharpFunctionalExtensions;
using Hng.Application.Features.UserManagement.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hng.Application.Features.UserManagement.Handlers
{
    public class VerifyForgotPasswordCodeHandler : IRequestHandler<VerifyForgotPasswordCodeDto, Result<VerifyForgotPasswordCodeResponse>>
    {
        private readonly IRepository<User> _userRepo;

        public VerifyForgotPasswordCodeHandler(IRepository<User> userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<Result<VerifyForgotPasswordCodeResponse>> Handle(VerifyForgotPasswordCodeDto request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetBySpec(u => u.Email == request.Email && u.PasswordResetToken == request.Code);

            if (user == null)
                return Result.Failure<VerifyForgotPasswordCodeResponse>("This code has been used!");

            if ((DateTime.UtcNow - user.PasswordResetTokenTime.GetValueOrDefault()).Minutes > 10)
                return Result.Failure<VerifyForgotPasswordCodeResponse>("Code has expired!");

            return Result.Success(new VerifyForgotPasswordCodeResponse()
            {
                Message = "successful",
                StatusCode = StatusCodes.Status200OK,
                Data = new VerifyForgotPasswordCodeData()
                {
                    Message = "success"
                }
            });
        }
    }
}