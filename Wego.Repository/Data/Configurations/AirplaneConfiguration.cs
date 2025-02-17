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
    internal class AirplaneConfiguration : IEntityTypeConfiguration<Airplane>
    {
        public void Configure(EntityTypeBuilder<Airplane> builder)
        {
            builder
              .HasOne(a => a.Airline)
              .WithMany(al => al.Airplanes)
              .HasForeignKey(a => a.AirlineId)
              .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
