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
    public class SeatReservationConfiguration : IEntityTypeConfiguration<SeatReservation>
    {
        public void Configure(EntityTypeBuilder<SeatReservation> builder)
        {
            builder.Property(e => e.Id);

            builder.HasOne(d => d.FlightBooking)
                .WithMany(p => p.SeatReservations)
                .HasForeignKey(d => d.FlightBookingId);
        }
    }
}
