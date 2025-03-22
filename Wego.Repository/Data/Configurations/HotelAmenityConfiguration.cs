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
    public class HotelAmenityConfiguration : IEntityTypeConfiguration<HotelAmenity>
    {
        public void Configure(EntityTypeBuilder<HotelAmenity> builder)
        {
            // تعريف المفتاح الأساسي المركب
            builder.HasKey(ha => new { ha.HotelId, ha.AmenityId });

            // العلاقة مع Hotel
            builder.HasOne(ha => ha.Hotel)
                .WithMany(h => h.HotelAmenities)
                .HasForeignKey(ha => ha.HotelId);

            // العلاقة مع Amenity
            builder.HasOne(ha => ha.Amenity)
                .WithMany(a => a.HotelAmenities) 
                .HasForeignKey(ha => ha.AmenityId);
        }
    }


}
