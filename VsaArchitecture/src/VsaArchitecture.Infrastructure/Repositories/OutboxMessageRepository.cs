using VsaArchitecture.Application.Contracts.Infrastructure.Persistent;
using VsaArchitecture.Domain.Entities;

namespace VsaArchitecture.Infrastructure.Repositories;

public class OutboxMessageRepository(ApplicationDbContext context)
    : BaseRepository<OutboxMessage>(context), IOutboxMessageRepository
{
}