namespace Conduit.Application.Abstractions.Results;

public sealed record ValidationError(string Property, string Code, string Message);
