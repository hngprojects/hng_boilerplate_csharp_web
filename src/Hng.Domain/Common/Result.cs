namespace Hng.Domain.Common;

public class Result<T>
{
    public bool IsSuccess { get; }

    public T Value { get; }

    public Error Error { get; }

    private Result(bool success, T data, Error error)
    {
        IsSuccess = success;
        Value = data;
        Error = error;
    }

    public static Result<T> Success(T value) => new(true, value, null);
    public static Result<T> Failure(Error error) => new(false, default, error);
}
