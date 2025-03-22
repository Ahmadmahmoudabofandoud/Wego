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

    public class AirplaneConfiguration : IEntityTypeConfiguration<Airplane>
    {
        public void Configure(EntityTypeBuilder<Airplane> entity)
        {
            entity.Property(e => e.Id);

            entity.Property(e => e.Code)
                .HasMaxLength(50);

            entity.Property(e => e.Type)
                .HasMaxLength(255);

            entity.HasOne(d => d.Airline)
                .WithMany(p => p.Airplanes)
                .HasForeignKey(d => d.AirlineId);
        }
    }
}
