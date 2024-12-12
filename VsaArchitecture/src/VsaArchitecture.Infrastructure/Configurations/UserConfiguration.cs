using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsaArchitecture.Domain.Entities;

namespace VsaArchitecture.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users", "dbo");

            BaseEntityConfiguration.ConfigureAuditable(builder);

            builder.HasKey(x => x.Id);
            builder.Property(p => p.UserId)
                .HasMaxLength(20)
                .IsRequired(true);
            builder.Property(p => p.Password)
                .HasMaxLength(15)
                .IsRequired(true);

            builder.HasIndex(p => p.UserId);
        }
    }
}
