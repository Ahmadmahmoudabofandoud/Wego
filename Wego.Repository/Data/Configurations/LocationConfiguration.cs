using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models;

namespace Wego.Repository.Data.Configurations
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.Property(e => e.Id);

            builder.Property(e => e.City)
                .HasMaxLength(255);

            builder.Property(e => e.Country)
                .HasMaxLength(255);

            builder.Property(e => e.Image)
                .HasMaxLength(255);
        }
    }

}
