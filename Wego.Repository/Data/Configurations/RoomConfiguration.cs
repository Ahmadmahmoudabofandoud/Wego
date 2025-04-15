using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models.Hotels;

namespace Wego.Repository.Data.Configurations
{
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.Property(e => e.Id);


            builder.Property(e => e.RoomLocation)
                .HasMaxLength(255);

            builder.Property(e => e.RoomDescribtion)
                .HasMaxLength(255);

            builder.Property(e => e.RoomTitle)
                .HasMaxLength(255);

            builder.Property(e => e.RoomType)
                .HasMaxLength(255);

            builder.HasOne(d => d.Hotel)
                .WithMany(p => p.Rooms)
                .HasForeignKey(d => d.HotelId);
        }
    }

}
