using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models.Booking;

namespace Wego.Repository.Data.Configurations
{
    internal class SeatReservationConfiguration : IEntityTypeConfiguration<SeatReservation>
    {
        public void Configure(EntityTypeBuilder<SeatReservation> builder)
        {
            builder
             .HasOne(sr => sr.FlightBooking)
             .WithMany(fb => fb.SeatReservations)
             .HasForeignKey(sr => sr.FlightBookingId)
             .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(sr => sr.Seat)
                .WithMany(s => s.SeatReservations)
                .HasForeignKey(sr => sr.SeatId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
