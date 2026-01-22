namespace Conduit.Domain.Entities.Common;

public sealed record Error(
    string Code,
    string Message,
    IReadOnlyCollection<ValidationError>? ValidationErrors = null
)
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public static Error Validation(IReadOnlyCollection<ValidationError> errors) =>
        new("validation.error", "One or more validation errors occurred.", errors);

    public static Error NotFound(string code, string description) => new(code, description);
}
