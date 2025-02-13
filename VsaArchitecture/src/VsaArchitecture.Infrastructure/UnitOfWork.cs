using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using VsaArchitecture.Application.Contracts.Infrastructure.Persistent;
using VsaArchitecture.Domain.Common;
using VsaArchitecture.Domain.Entities;

namespace VsaArchitecture.Infrastructure;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> CommitAsync()
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            foreach (var entry in _context.ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        {

                            entry.Entity.CreatedBy = string.Empty;// _currentUserService.UserId ?? Guid.NewGuid();
                            entry.Entity.Created = DateTime.Now;
                            break;
                        }
                    case EntityState.Modified:
                        {
                            entry.Entity.LastModifiedBy = string.Empty;// _currentUserService.UserId;
                            entry.Entity.LastModified = DateTime.Now;
                            break;
                        }
                    case EntityState.Deleted:
                        break;
                }
            }
            var affectedRows = await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return affectedRows;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }
        finally
        {
            await _context.Database.CloseConnectionAsync();
            await _context.DisposeAsync();
        }
    }

    public void Dispose()
    {
        _context?.Dispose();
        GC.SuppressFinalize(this);
    }
}
