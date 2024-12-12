namespace VsaArchitecture.Application.Contracts.Infrastructure.Persistent;

public interface IUnitOfWork
{
    Task<int> CommitAsync();
}
