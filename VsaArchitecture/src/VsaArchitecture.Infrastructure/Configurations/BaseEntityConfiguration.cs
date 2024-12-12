using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsaArchitecture.Domain.Common;

namespace VsaArchitecture.Infrastructure.Configurations;

public static class BaseEntityConfiguration 
{
    public static void ConfigureAuditable<TEntity>(EntityTypeBuilder<TEntity> builder) where TEntity : AuditableEntity
    {
        builder.Property(p => p.Created)
            .HasColumnType("datetime")
            .IsRequired();
        builder.Property(p => p.CreatedBy)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(p => p.LastModified)
            .HasColumnType("datetime");
        builder.Property(p => p.LastModifiedBy)
            .HasMaxLength(20);
    }
}
