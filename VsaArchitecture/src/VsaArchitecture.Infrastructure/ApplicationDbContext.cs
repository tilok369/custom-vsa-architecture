using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using VsaArchitecture.Domain.Entities;

namespace VsaArchitecture.Infrastructure;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<OutboxMessage> OutboxMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var savedEntities = ChangeTracker.Entries()
            .Where(IsOutboxEnabled)
            .Select(e => new {e.Entity, e.State})
            .ToList();

        int affectedRows = await base.SaveChangesAsync(cancellationToken); // Save changes first to generate IDs
        
        if(affectedRows == 0)
            return affectedRows;
        var outboxMessages = savedEntities
            .Select(entry => CreateOutboxMessage(entry.Entity, entry.State)).ToList();

        if (!outboxMessages.Any()) return affectedRows;
        
        await Set<OutboxMessage>().AddRangeAsync(outboxMessages, cancellationToken);
        affectedRows += (await base.SaveChangesAsync(cancellationToken)); // Save outbox messages

        return affectedRows;
    }
    
    private bool IsOutboxEnabled(EntityEntry entry)
    {
        return (entry.State == EntityState.Added || entry.State == EntityState.Modified ||
                entry.State == EntityState.Deleted)
               && entry.Entity.GetType() == typeof(User);
    }

    private OutboxMessage CreateOutboxMessage(Object entity, EntityState state)
    {
        return new OutboxMessage
        {
            EventType = $"{entity.GetType().Name}_{state}",
            Message = JsonSerializer.Serialize(entity),
            PostedOn = DateTime.UtcNow
        };
    }
}
