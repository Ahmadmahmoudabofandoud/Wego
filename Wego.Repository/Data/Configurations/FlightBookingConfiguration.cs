using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models.Booking;

namespace Wego.Repository.Data.Configurations
{

    public class FlightBookingConfiguration : IEntityTypeConfiguration<FlightBooking>
    {
        public void Configure(EntityTypeBuilder<FlightBooking> entity)
        {
            entity.Property(e => e.Id);

            entity.Property(e => e.BookingDate).HasColumnType("datetime");

            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50);

            entity.Property(e => e.Status)
                .HasMaxLength(255);

            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Flight)
                .WithMany(p => p.FlightBookings)
                .HasForeignKey(d => d.FlightId);

            entity.HasOne(d => d.User)
                .WithMany(p => p.FlightBookings)
                .HasForeignKey(d => d.UserId);
        }
    }
}
