using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsaArchitecture.Application.Common.Filters;

namespace VsaArchitecture.Application.Common.Extensions;

public static class RouteHandlerBuilderExtensions
{
    public static RouteHandlerBuilder WithPostRequest<TRequest>(
        this RouteHandlerBuilder builder, 
        string summary = "", 
        bool authRequired = true)
    {
        builder.Produces(StatusCodes.Status200OK)
            .AddEndpointFilter<ValidationFilter<TRequest>>()
            .ProducesValidationProblem();
        if(!string.IsNullOrEmpty(summary))
            builder.WithSummary(summary);
        if(authRequired)
            builder.RequireAuthorization();

        return builder;
    }

    public static RouteHandlerBuilder WithGetRequest(
        this RouteHandlerBuilder builder, 
        string summary = "", 
        bool authRequired = true)
    {
        builder.Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
        if (!string.IsNullOrEmpty(summary))
            builder.WithSummary(summary);
        if (authRequired)
            builder.RequireAuthorization();

        return builder;
    }
}
