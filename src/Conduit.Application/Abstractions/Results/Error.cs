namespace Conduit.Application.Abstractions.Results;

public sealed record Error(
    string Code,
    string Message,
    IReadOnlyCollection<ValidationError>? ValidationErrors = null
)
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public static Error Validation(IReadOnlyCollection<ValidationError> errors) =>
        new("Validation.Error", "One or more validation errors occurred.", errors);
}
