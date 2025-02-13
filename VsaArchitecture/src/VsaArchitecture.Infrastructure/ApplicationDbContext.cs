using Microsoft.EntityFrameworkCore;
using System.Reflection;
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
}
