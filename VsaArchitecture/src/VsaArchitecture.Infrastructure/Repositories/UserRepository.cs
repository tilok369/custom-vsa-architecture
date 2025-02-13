using VsaArchitecture.Application.Contracts.Infrastructure.Persistent;
using VsaArchitecture.Domain.Entities;

namespace VsaArchitecture.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext context) : BaseRepository<User>(context), IUserRepository;
