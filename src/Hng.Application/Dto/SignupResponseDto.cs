

namespace Hng.Application.Dto
{
    public class SignupResponseDto
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