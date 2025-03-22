using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wego.Core.Models.Flights;

namespace Wego.Repository.Data.Configurations
{
    public class FeatureConfiguration : IEntityTypeConfiguration<Feature>
    {
        public void Configure(EntityTypeBuilder<Feature> entity)
        {
            entity.Property(e => e.Id);

            entity.HasOne(d => d.Airplane)
                .WithMany(p => p.Features)
                .HasForeignKey(d => d.AirplaneId);
        }
    }

}
