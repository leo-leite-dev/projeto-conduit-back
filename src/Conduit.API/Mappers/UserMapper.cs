using Conduit.Api.Authentication.Contracts.Auth.Login;
using Conduit.Api.Authentication.Contracts.Auth.Register;

namespace Conduit.Api.Contracts.Users;

public static class UserMapper
{
    public static UserResponse FromRegister(
        RegisterUserRequest request,
        AuthRegisterResponse result
    )
    {
        return new UserResponse
        {
            User = new UserDto
            {
                Email = request.User.Email,
                Username = request.User.Username,
                Token = result.AccessToken,
            },
        };
    }

    public static UserResponse FromLogin(LoginUserRequest request, AuthLoginResponse result)
    {
        return new UserResponse
        {
            User = new UserDto
            {
                Email = request.User.Email,
                Username = request.User.Email,
                Token = result.AccessToken,
            },
        };
    }
}
