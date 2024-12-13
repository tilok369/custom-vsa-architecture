using Asp.Versioning;
using Asp.Versioning.Builder;
using Carter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsaArchitecture.Application.Common.Extensions;

public class BaseCarterModule : CarterModule
{
    public ApiVersionSet ApiVersionSet { get; private set; } = null!;

    public BaseCarterModule()
        :base("api/v{apiVersion:apiVersion}")
    {
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        ApiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();
    }
}
