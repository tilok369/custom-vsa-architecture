using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using VsaArchitecture.Application.Common.Behavior;
using VsaArchitecture.Application.Contracts.Services;
using VsaArchitecture.Application.Services;

namespace VsaArchitecture.Application;

public static class ConfigureApplicationServices
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(typeof(ConfigureApplicationServices).Assembly);

            options.AddOpenBehavior(typeof(PerformanceBehavior<,>));
            options.AddOpenBehavior(typeof(AuthorizationBehavior<,>));
            options.AddRequestPreProcessor(typeof(IRequestPreProcessor<>),typeof(LoggingBehavior<>));
        });

        return services;
    }
}
