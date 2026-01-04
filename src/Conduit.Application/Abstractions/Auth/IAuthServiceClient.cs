namespace Conduit.Application.Abstractions.Auth;

public interface IAuthServiceClient
{
    Task<AuthLoginResult> LoginAsync(AuthLoginCommand command,  CancellationToken cancellationToken = default);
    Task<AuthRegisterResult> RegisterAsync(AuthRegisterCommand command,CancellationToken cancellationToken = default);
}