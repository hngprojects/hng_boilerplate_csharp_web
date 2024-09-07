using Hng.Application.Features.UserManagement.Commands;
using Hng.Application.Features.UserManagement.Dtos;
using HotChocolate.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hng.Graphql
{
    public partial class Mutations
    {
        public async Task<UserLoginResponseDto<SignupResponseData>> Login(UserLoginRequestDto loginRequest, [FromServices] IMediator mediator)
        {
            var command = new CreateUserLoginCommand(loginRequest);
            return await mediator.Send(command);
        }

        public async Task<UserLoginResponseDto<SignupResponseData>> GoogleLogin(GoogleLoginRequestDto googleLoginRequest, [FromServices] IMediator mediator)
        {
            var command = new GoogleLoginCommand(googleLoginRequest.IdToken);
            return await mediator.Send(command);
        }

        public async Task<SignUpResponse> UserSignUp(UserSignUpDto body, [FromServices] IMediator mediator)
        {
            var command = new UserSignUpCommand(body);
            return await mediator.Send(command);
        }

        [Authorize]
        public async Task<ChangePasswordResponse> ChangePassword([FromBody] ChangePasswordCommand command, [FromServices] IMediator mediator)
        {
            var response = await mediator.Send(command);
            return response.Value;
        }

        public async Task<ForgotPasswordResponse> ForgotPassword([FromBody] ForgotPasswordRequestDto request, [FromServices] IMediator mediator)
        {
            var response = await mediator.Send(new ForgotPasswordDto(request.Email, false));
            return response.Value;
        }

        public async Task<ForgotPasswordResponse> ForgotPasswordMobile([FromBody] ForgotPasswordRequestDto request, [FromServices] IMediator mediator)
        {
            var response = await mediator.Send(new ForgotPasswordDto(request.Email, true));
            return response.Value;
        }

        public async Task<VerifyForgotPasswordCodeResponse> VerifyForgotPasswordCode([FromBody] VerifyForgotPasswordCodeDto request, [FromServices] IMediator mediator)
        {
            var response = await mediator.Send(request);
            return response.Value;
        }

        public async Task<PasswordResetMobileResponse> PasswordResetMobile([FromBody] PasswordResetMobileDto request, [FromServices] IMediator mediator)
        {
            var respone = await mediator.Send(new PasswordResetMobileCommand(request));
            return respone.Value;
        }

        [Authorize]
        public async Task<PasswordResetResponse> PasswordReset([FromBody] PasswordResetDto request, [FromServices] IMediator mediator)
        {
            var response = await mediator.Send(request);
            return response.Value;
        }

        public async Task<UserLoginResponseDto<object>> FacebookLogin([FromBody] FacebookLoginRequestDto request, [FromServices] IMediator mediator)
        {
            var command = new FacebookLoginCommand(request.AccessToken);
            return await mediator.Send(command);
        }
    }
}