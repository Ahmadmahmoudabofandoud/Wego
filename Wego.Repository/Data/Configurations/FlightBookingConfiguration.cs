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
    internal class FlightBookingConfiguration : IEntityTypeConfiguration<FlightBooking>
    {
        public void Configure(EntityTypeBuilder<FlightBooking> builder)
        {
            builder
                .HasOne(fb => fb.User)
                .WithMany(u => u.FlightBookings)
                .HasForeignKey(fb => fb.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasOne(fb => fb.Flight)
                .WithMany(f => f.FlightBookings)
                .HasForeignKey(fb => fb.FlightId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
