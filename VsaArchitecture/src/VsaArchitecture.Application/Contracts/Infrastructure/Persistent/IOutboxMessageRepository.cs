using VsaArchitecture.Domain.Entities;

namespace VsaArchitecture.Application.Contracts.Infrastructure.Persistent;

public interface IOutboxMessageRepository: IBaseRepository<OutboxMessage>
{
    
}