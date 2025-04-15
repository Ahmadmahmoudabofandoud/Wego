using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models.Flights;

namespace Wego.Repository.Data.Configurations
{
    public class AirlineConfiguration : IEntityTypeConfiguration<Airline>
    {
        public void Configure(EntityTypeBuilder<Airline> entity)
        {
            entity.Property(e => e.Id);

            entity.Property(e => e.Code)
                .HasMaxLength(50);

            entity.Property(e => e.Name)
                .HasMaxLength(255);

            entity.Property(e => e.Image)
                .HasMaxLength(255);
        }
    }
}
