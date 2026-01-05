using System.Net;

namespace Conduit.API.Exceptions;

public sealed class BffHttpException : Exception
{
    public HttpStatusCode StatusCode { get; }
    public string Type { get; }

    public BffHttpException(HttpStatusCode statusCode, string type, string message)
        : base(message)
    {
        StatusCode = statusCode;
        Type = type;
    }
}
