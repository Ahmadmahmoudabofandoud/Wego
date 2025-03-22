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
    public class HotelBookingConfiguration : IEntityTypeConfiguration<HotelBooking>
    {
        public void Configure(EntityTypeBuilder<HotelBooking> builder)
        {
            builder.Property(e => e.Id);

            builder.Property(e => e.BookingDate).HasColumnType("datetime");
            builder.Property(e => e.Status)
                .HasMaxLength(255)
                .IsUnicode(false);
            builder.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");

            builder.HasOne(d => d.User)
                .WithMany(p => p.HotelBookings)
                .HasForeignKey(d => d.UserId);
        }
    }

}
