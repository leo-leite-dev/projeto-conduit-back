using Conduit.Application.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Conduit.Application.DependencyInjection;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ApplicationDependencyInjection).Assembly);
        });

        services.AddValidatorsFromAssembly(typeof(ApplicationDependencyInjection).Assembly);

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));

        return services;
    }
}
