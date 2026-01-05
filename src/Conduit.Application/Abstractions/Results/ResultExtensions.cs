namespace Conduit.Application.Abstractions.Results;

public static class ResultExtensions
{
    public static Result<T> ToFailure<T>(this Result result, Error error) =>
        Result<T>.Failure(error);
}
