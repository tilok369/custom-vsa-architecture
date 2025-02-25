﻿using Asp.Versioning.Builder;
using Asp.Versioning;
using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using VsaArchitecture.Application.Common.Extensions;
using VsaArchitecture.Application.Common.Filters;
using VsaArchitecture.Application.Contracts.Infrastructure.Persistent;
using VsaArchitecture.Domain.Entities;

namespace VsaArchitecture.Application.Features.Users
{
    public static class CreateUser
    {
        public record CreateUserCommand : IRequest<int>
        {
            public string UserId { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
        {
            public CreateUserCommandValidator()
            {
                RuleFor(p=>p.UserId)
                    .NotEmpty()
                    .MaximumLength(20);

                RuleFor(p => p.Password)
                    .NotEmpty()
                    .MaximumLength(15);
            }
        }

        public class CreateUserHandler : IRequestHandler<CreateUserCommand, int>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IUserRepository _userRepository;

            public CreateUserHandler(
                IUnitOfWork unitOfWork,
                IUserRepository userRepository)
            {
                _unitOfWork = unitOfWork;
                _userRepository = userRepository;
            }

            public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                var user = request.Adapt<User>();
                await _userRepository.AddAsync(user);
                var affectedRows = await _unitOfWork.CommitAsync();
                return user.Id;
            }
        }
    }

    public class CreateUserEndpoints : BaseCarterModule
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            base.AddRoutes(app);

            app.MapPost("users", async (CreateUser.CreateUserCommand command, ISender sender) =>
            {
                var userId = await sender.Send(command);
                return Results.Ok(userId);
            })
            .WithPostRequest<CreateUser.CreateUserCommand>("Create an user")
            .WithApiVersionSet(ApiVersionSet);
        }
    }
}
