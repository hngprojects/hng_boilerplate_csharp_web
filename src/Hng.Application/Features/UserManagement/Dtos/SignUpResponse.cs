namespace Hng.Application.Features.UserManagement.Dtos
{
    public class SignUpResponse
    {
        public string Message { get; set; }
        public SignupResponseData Data { get; set; }
    }

    public class SignupResponseData
    {
        public string Token { get; set; }
        public UserResponseDto User { get; set; }
    }
}
