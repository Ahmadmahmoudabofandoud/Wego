using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models.Flights;

namespace Wego.Repository.Data.Configurations
{
    public class AirportConfiguration : IEntityTypeConfiguration<Airport>
    {
        public void Configure(EntityTypeBuilder<Airport> entity)
        {
            entity.Property(e => e.Id);

            entity.Property(e => e.Code)
                .HasMaxLength(50);

            entity.Property(e => e.Name)
                .HasMaxLength(255);

            entity.HasOne(d => d.Location)
                .WithMany(p => p.Airports)
                .HasForeignKey(d => d.LocationId);
        }
    }
}
