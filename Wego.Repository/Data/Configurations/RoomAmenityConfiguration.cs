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
    public class RoomAmenityConfiguration : IEntityTypeConfiguration<RoomAmenity>
    {
        public void Configure(EntityTypeBuilder<RoomAmenity> builder)
        {
            builder.HasNoKey();

            builder.HasOne(d => d.Amenity)
                .WithMany()
                .HasForeignKey(d => d.AmenityId);

            builder.HasOne(d => d.Room)
                .WithMany()
                .HasForeignKey(d => d.RoomId);
        }
    }

}
