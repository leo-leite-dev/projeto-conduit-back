using Conduit.Application.Abstractions.Results;

namespace Conduit.Application.Errors;

public static class AuthErrors
{
    public static readonly Error Unauthorized = new(
        "Auth.Unauthorized",
        "Usuário não autenticado."
    );
}
