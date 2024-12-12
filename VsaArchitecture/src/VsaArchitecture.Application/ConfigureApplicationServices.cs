using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VsaArchitecture.Application.Common.Behavior;
using VsaArchitecture.Application.Contracts.Services;

namespace VsaArchitecture.Application;

public static class ConfigureApplicationServices
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserService, ICurrentUserService>();

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
