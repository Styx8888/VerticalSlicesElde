namespace PipeReports.API.Shared.Results;

public class Result<T> : Result
{
    private readonly T _value;
    internal Result(T value, bool success, Error error)
        : base(success, error)
    {
        _value = value;
    }

    public T Value => IsSuccess ? _value : throw new InvalidOperationException("No value for failure result");

    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator Result<T>(Error error) => Failure<T>(error);
}
