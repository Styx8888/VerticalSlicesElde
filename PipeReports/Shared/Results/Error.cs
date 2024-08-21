namespace PipeReports.API.Shared.Results;

public sealed record Error(string Code, string? Message = null)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static Error NotFound(string? details) => new("Error.NotFound", details);
    public static Error Validation(string validationResult) => new("Error.Validation", validationResult);
}