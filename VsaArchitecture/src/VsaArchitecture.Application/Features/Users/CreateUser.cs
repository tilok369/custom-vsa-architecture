using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using VsaArchitecture.Application.Common.Filters;
using VsaArchitecture.Application.Contracts.Infrastructure.Persistent;
using VsaArchitecture.Domain.Entities;

namespace VsaArchitecture.Application.Features.Users
{
    public static class CreateUser
    {
        public record Command : IRequest<int>
        {
            public string UserId { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p=>p.UserId)
                    .NotEmpty()
                    .MaximumLength(20);

                RuleFor(p => p.Password)
                    .NotEmpty()
                    .MaximumLength(15);
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IUserRepository _userRepository;

            public Handler(
                IUnitOfWork unitOfWork,
                IUserRepository userRepository)
            {
                _unitOfWork = unitOfWork;
                _userRepository = userRepository;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = request.Adapt<User>();
                await _userRepository.AddAsync(user);
                var affectedRows = await _unitOfWork.CommitAsync();
                return user.Id;
            }
        }
    }

    public class CreateUserEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("api/users", async (CreateUser.Command command, ISender sender) =>
            {
                var userId = await sender.Send(command);
                return Results.Ok(userId);
            })
            .WithDescription("Create an user")
            .AddEndpointFilter<ValidationFilter<CreateUser.Command>>()
            .Produces(StatusCodes.Status200OK)
            .ProducesValidationProblem();
        }
    }
}
