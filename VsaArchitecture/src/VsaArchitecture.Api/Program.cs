using Serilog;
using VsaArchitecture.Infrastructure;
using VsaArchitecture.Application;
using Carter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Asp.Versioning;
using VsaArchitecture.Api;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
builder.Services.AddSingleton(Log.Logger);

builder.Services.AddHttpContextAccessor();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddCarter();

builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", host =>
        {
            host.Username("guest");
            host.Password("guest");
        });
        cfg.ConfigureEndpoints(context);
    });
});

//Enable CORS//Cross site resource sharing
builder.Services.AddCors(options =>
{
    options.AddPolicy("VsaArchitectureCorsPolicy",
        b => b.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
    );
});

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1);
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
})
    .AddApiExplorer(options => 
    {
        options.GroupNameFormat = "'v'V";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                    };
                });

builder.Services.AddAuthorization();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
    {
         new OpenApiSecurityScheme
         {
           Reference = new OpenApiReference
           {
             Type = ReferenceType.SecurityScheme,
             Id = "Bearer"
           }
          },
         new string[] { }
    }
  });
});

builder.Services.AddCustomHealthChecks(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("VsaArchitectureCorsPolicy");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapCarter();

app.MapHealthChecks("/health", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
