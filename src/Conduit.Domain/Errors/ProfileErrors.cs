using Conduit.Domain.Entities.Common;

namespace Conduit.Domain.Errors;

public static class ProfileErrors
{
    public static readonly Error NotFound = Error.NotFound(
        code: "profile.not_found",
        description: "Profile não encontrado."
    );

    public static readonly Error AlreadyExists = Error.NotFound(
        code: "profile.already_exists",
        description: "Profile já existe."
    );
}
