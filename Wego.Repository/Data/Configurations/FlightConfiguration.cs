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

    public class FlightConfiguration : IEntityTypeConfiguration<Flight>
    {
        public void Configure(EntityTypeBuilder<Flight> entity)
        {
            entity.Property(e => e.Id);

            entity.Property(e => e.ArrivalTime).HasColumnType("datetime");
            entity.Property(e => e.DepartureTime).HasColumnType("datetime");

            entity.Property(e => e.BusinessClassPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.EconomyClassPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.FirstClassPrice).HasColumnType("decimal(10, 2)");

            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Airline)
                .WithMany(p => p.Flights)
                .HasForeignKey(d => d.AirlineId);

            entity.HasOne(d => d.Airplane)
                .WithMany(p => p.Flights)
                .HasForeignKey(d => d.AirplaneId)
                   .OnDelete(DeleteBehavior.NoAction);


            entity.HasOne(d => d.ArrivalAirport)
                .WithMany(p => p.FlightArrivalAirports)
                .HasForeignKey(d => d.ArrivalAirportId);

            entity.HasOne(d => d.DepartureAirport)
                .WithMany(p => p.FlightDepartureAirports)
                .HasForeignKey(d => d.DepartureAirportId)
                    .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
