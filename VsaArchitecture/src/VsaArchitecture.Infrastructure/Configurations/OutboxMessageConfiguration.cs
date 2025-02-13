using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VsaArchitecture.Domain.Entities;

namespace VsaArchitecture.Infrastructure.Configurations;

public class OutboxMessageConfiguration: IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("OutboxMessages", "dbo");

        builder.HasKey(x => x.Id);
        builder.Property(p => p.EventType)
            .HasMaxLength(50);
        builder.Property(p => p.Message)
            .HasMaxLength(8000);
        builder.Property(p => p.Error)
            .HasMaxLength(8000);
    }
}