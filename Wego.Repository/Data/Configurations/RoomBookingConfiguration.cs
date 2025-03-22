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
    public class RoomBookingConfiguration : IEntityTypeConfiguration<RoomBooking>
    {
        public void Configure(EntityTypeBuilder<RoomBooking> builder)
        {
            builder.ToTable("RoomBooking");

            builder.Property(e => e.Id);

            builder.Property(e => e.Checkin).HasColumnType("datetime");

            builder.Property(e => e.Checkout).HasColumnType("datetime");

            builder.HasOne(d => d.Booking)
                .WithMany(p => p.RoomBookings)
                .HasForeignKey(d => d.BookingId);

            builder.HasOne(d => d.Room)
                .WithMany(p => p.RoomBookings)
                .HasForeignKey(d => d.RoomId);
        }
    }

}
