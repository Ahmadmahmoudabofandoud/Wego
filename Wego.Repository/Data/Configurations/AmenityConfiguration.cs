using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wego.Core.Models.Hotels;

namespace Wego.Repository.Data.Configurations
{
    public class AmenityConfiguration : IEntityTypeConfiguration<Amenity>
    {
        public void Configure(EntityTypeBuilder<Amenity> entity)
        {
            entity.HasKey(e => e.Id); // التأكد من أن Id هو Primary Key
            
            entity.Property(a => a.Id)
                   .ValueGeneratedOnAdd()
                   .UseIdentityColumn();

            entity.Property(e => e.Name)
                  .HasMaxLength(255)
                  .IsRequired();
        }
    }
}
