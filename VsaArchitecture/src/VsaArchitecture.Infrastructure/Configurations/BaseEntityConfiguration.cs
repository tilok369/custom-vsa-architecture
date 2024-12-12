using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsaArchitecture.Domain.Common;

namespace VsaArchitecture.Infrastructure.Configurations;

public class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : AuditableEntity
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(p => p.Created)
            .HasColumnType("datetime")
            .IsRequired();
        builder.Property(p => p.CreatedBy)
            .HasColumnType("nvarchar")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(p => p.LastModified)
            .HasColumnType("datetime");
        builder.Property(p => p.LastModifiedBy)
            .HasColumnType("nvarchar")
            .HasMaxLength(20);
    }
}
