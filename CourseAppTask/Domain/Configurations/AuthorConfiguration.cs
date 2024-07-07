using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Configurations
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {

            builder.Property(m => m.FullName)
               .IsRequired()
               .HasMaxLength(50);
            builder.Property(m => m.Country)
                .IsRequired()
                .HasMaxLength(40);

            builder.Property(m => m.Age)
                .IsRequired();
        }
    }
}
