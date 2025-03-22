using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models;

namespace Wego.Repository.Data.Configurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.Property(e => e.Id);

            builder.Property(e => e.Url)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("URL");

            builder.HasOne(d => d.Airline)
                .WithOne(p => p.Images)
                .HasForeignKey<Image>(d => d.AirlineId);



            builder.HasOne(d => d.Hotel)
                .WithMany(p => p.Images)
                .HasForeignKey(d => d.HotelId);

            builder.HasOne(d => d.Room)
                .WithMany(p => p.Images)
                .HasForeignKey(d => d.RoomId);
        }
    }

}
