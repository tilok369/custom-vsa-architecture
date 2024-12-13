using Asp.Versioning.Builder;
using Carter;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VsaArchitecture.Application.Common.Extensions;
using VsaArchitecture.Application.Common.Filters;
using VsaArchitecture.Application.Features.Users;

namespace VsaArchitecture.Application.Features.Authentication
{
    public static class UserAuthentication
    {
        public record LoginCommand : IRequest<LoginResponse>
        {
            public string UserId { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public record LoginResponse
        {
            public bool Authenticated { get; set; }
            public string UserId { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string Token { get; set; } = string.Empty;
            public List<string> Roles { get; set; } = new();
        }

        public class LoginCommandValidator : AbstractValidator<LoginCommand>
        {
            public LoginCommandValidator()
            {
                RuleFor(p => p.UserId)
                        .NotEmpty();

                RuleFor(p => p.Password)
                    .NotEmpty();
            }
        }

        public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
        {
            private readonly IConfiguration _configuration;

            public LoginHandler(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                //TODO: apply your authentication logic here
                if (request.UserId == "admin" && request.Password == "test")
                {
                    return new LoginResponse
                    {
                        Authenticated = true,
                        UserId = request.UserId,
                        Token = GenerateJwtToken(request.UserId),
                        Name = "Admin",
                        Roles = new List<string> { "Admin" }
                    };
                }

                return new LoginResponse { Authenticated = false };
            }

            private string GenerateJwtToken(string userId)
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

                var token = new JwtSecurityToken(issuer: _configuration["Jwt:Issuer"], audience: _configuration["Jwt:Issuer"],
                    claims: claims, expires: DateTime.Now.AddMinutes(int.Parse(_configuration["Jwt:ExpiryMinutes"])),
                    signingCredentials: credentials);

                var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
                return encodedToken;
            }
        }
    }

    public class UserAuthenticationEndpoints : BaseCarterModule
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            base.AddRoutes(app);

            app.MapPost("api/login", async (UserAuthentication.LoginCommand command, ISender sender) =>
            {
                var result = await sender.Send(command);

                if (!result.Authenticated)
                    return Results.Unauthorized();

                return Results.Ok(result);
            })
            .WithPostRequest<UserAuthentication.LoginCommand>("User login", false)
            .WithApiVersionSet(ApiVersionSet);
        }
    }
}
