using FluentValidation;
using MediatR.Pipeline;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using VsaArchitecture.Application.BackgroundServices;
using VsaArchitecture.Application.Common.Behavior;
using VsaArchitecture.Application.Contracts.BackgroundServices;
using VsaArchitecture.Application.Contracts.Services;
using VsaArchitecture.Application.Services;

namespace VsaArchitecture.Application;

public static class ConfigureApplicationServices
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<FluentValidatorHelper>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IOutboxBackgroundService, OutboxBackgroundService>();

        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(typeof(ConfigureApplicationServices).Assembly);

            options.AddOpenBehavior(typeof(PerformanceBehavior<,>));
            options.AddOpenBehavior(typeof(AuthorizationBehavior<,>));
            options.AddOpenBehavior(typeof(UnhandledExceptionBehaviour<,>));
            options.AddRequestPreProcessor(typeof(IRequestPreProcessor<>),typeof(LoggingBehavior<>));
        });

        return services;
    }
}
