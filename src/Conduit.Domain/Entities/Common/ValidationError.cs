namespace Conduit.Domain.Entities.Common;

public sealed record ValidationError(string Property, string Code, string Message);
