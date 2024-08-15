namespace PipeReports.API.Shared;

public class Result<T> : Result
{
    internal Result(T value, bool success, Error error)
        : base(success, error)
    {
        Value = value;
    }

    public T Value { get; set; }


    public static implicit operator Result<T>(T value)
    {
        return Success(value);
    }
}
