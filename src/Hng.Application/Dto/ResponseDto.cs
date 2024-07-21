namespace Hng.Application.Dto
{
    public record SuccessResponseDto(string message, int status_code, bool success = true);
    public record FailureResponseDto(string message, int status_code, bool success = false);
}
