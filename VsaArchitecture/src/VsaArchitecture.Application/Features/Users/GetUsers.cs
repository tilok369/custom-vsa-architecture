using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using VsaArchitecture.Application.Common.Extensions;
using VsaArchitecture.Application.Common.Security;
using VsaArchitecture.Application.Contracts.Infrastructure.Persistent;

namespace VsaArchitecture.Application.Features.Users
{
    public static class GetUsers
    {
        [Authorize(Roles = "Admin")]
        public record GetUserQuery : IRequest<GetUserResponse>
        {
            public int Id { get; set; }
        }

        public record GetUserResponse
        {
            public int Id { get; set; }
            public string UserId { get; set; } = string.Empty;
            public DateTime Created { get; set; }

            public string CreatedBy { get; set; } = string.Empty;

            public DateTime? LastModified { get; set; }

            public string? LastModifiedBy { get; set; }
        }

        public class GetUserHandler : IRequestHandler<GetUserQuery, GetUserResponse>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IUserRepository _userRepository;

            public GetUserHandler(IUnitOfWork unitOfWork, IUserRepository userRepository)
            {
                _unitOfWork = unitOfWork;
                _userRepository = userRepository;
            }

            public async Task<GetUserResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetAsync(request.Id);
                await _unitOfWork.CommitAsync();

                return user.Adapt<GetUserResponse>();
            }
        }
    }

    public class GetUsersEndpoints : BaseCarterModule
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            base.AddRoutes(app);

            app.MapGet("users/{id}", async (int id, ISender sender) =>
            {
                var query = new GetUsers.GetUserQuery { Id = id };
                var result = await sender.Send(query);

                if (result == null)
                    return Results.NotFound();

                return Results.Ok(result);
            })
            .WithGetRequest("Get an user")
            .WithApiVersionSet(ApiVersionSet);
        }
    }
}
