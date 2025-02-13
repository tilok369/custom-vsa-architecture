using System.Data.Common;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using VsaArchitecture.Domain.Entities;

namespace VsaArchitecture.Infrastructure.Interceptors;

public class OutboxInterceptor: DbTransactionInterceptor
{
    public override async ValueTask<InterceptionResult> TransactionCommittingAsync(DbTransaction transaction, TransactionEventData eventData,
        InterceptionResult result, CancellationToken cancellationToken = new CancellationToken())
    {
        var context = eventData.Context;
        if (context == null) return await base.TransactionCommittingAsync(transaction, eventData, result, cancellationToken);

        foreach (var entry in context.ChangeTracker.Entries())
        {
            var outboxMessage = new OutboxMessage
            {
                EventType = GetEventType(entry),
                Message = JsonSerializer.Serialize(GetEntityObject(entry)),
                PostedOn = DateTime.UtcNow
            };
            context.Set<OutboxMessage>().Add(outboxMessage);
        }
        
        return await base.TransactionCommittingAsync(transaction, eventData, result, cancellationToken);
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