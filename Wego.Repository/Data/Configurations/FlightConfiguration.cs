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
    internal class FlightConfiguration : IEntityTypeConfiguration<Flight>
    {
        public void Configure(EntityTypeBuilder<Flight> builder)
        {
            builder
                .HasOne(f => f.Airplane)
                .WithMany(a => a.Flights)
                .HasForeignKey(f => f.AirplaneId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(f => f.Airline)
                .WithMany(a => a.Flights)
                .HasForeignKey(f => f.AirlineId)
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasOne(f => f.DepartureTerminal)
                .WithMany(t => t.DepartureFlights)
                .HasForeignKey(f => f.DepartureTerminalId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(f => f.ArrivalTerminal)
                .WithMany(t => t.ArriveFlights)
                .HasForeignKey(f => f.ArrivalTerminalId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
