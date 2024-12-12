using VsaArchitecture.Application.Contracts.Infrastructure.Persistent;
using VsaArchitecture.Domain.Entities;

namespace VsaArchitecture.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }
}
