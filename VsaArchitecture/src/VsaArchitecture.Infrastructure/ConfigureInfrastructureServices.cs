﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsaArchitecture.Application.Contracts.Infrastructure.Persistent;
using VsaArchitecture.Infrastructure.Repositories;

namespace VsaArchitecture.Infrastructure;

public static class ConfigureInfrastructureServices
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("ConnectionString"))
                );

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
