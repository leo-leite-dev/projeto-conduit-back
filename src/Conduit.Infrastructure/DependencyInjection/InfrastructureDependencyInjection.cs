using Conduit.Application.Abstractions.Auth;
using Conduit.Application.Abstractions.Repositories;
using Conduit.Infrastructure.Auth;
using Conduit.Infrastructure.Persistence.Context;
using Conduit.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Conduit.Infrastructure.Persistence.DependencyInjection;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<ConduitDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
        );

        services.AddScoped<IArticleRepository, ArticleRepository>();
        services.AddScoped<IProfileRepository, ProfileRepository>();

        services.AddScoped<ICurrentUser, CurrentUser>();

        return services;
    }
}
