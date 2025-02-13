using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using VsaArchitecture.Domain.Entities;

namespace VsaArchitecture.Infrastructure.Interceptors;

public class OutboxInterceptor: SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, 
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var context = eventData.Context;
        if (context == null) return await base.SavingChangesAsync(eventData, result, cancellationToken);

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if ((entry.State == EntityState.Added || entry.State == EntityState.Modified ||
                 entry.State == EntityState.Deleted)
                && entry.GetType() == typeof(User))
            {
                var outboxMessage = new OutboxMessage
                {
                    EventType = GetEventType(entry),
                    Message = JsonSerializer.Serialize(GetEntityObject(entry)),
                    PostedOn = DateTime.UtcNow
                };
                await context.Set<OutboxMessage>().AddAsync(outboxMessage, cancellationToken);
            }
        }
        
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
    
    private Dictionary<string, object> GetEntityObject(EntityEntry entry)
    {
        return entry.Properties
            .ToDictionary(p => p.Metadata.Name, p => p.CurrentValue);
    }

    private string GetEventType(EntityEntry entry)
    {
        return $"{entry.Entity.GetType().Name}_{entry.State.ToString()}";
    }
}