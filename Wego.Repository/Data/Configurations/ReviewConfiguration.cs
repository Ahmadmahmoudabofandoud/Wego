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
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.Property(e => e.Id);

            builder.Property(e => e.Comment).HasColumnType("text");


            builder.Property(e => e.Rating).HasColumnType("decimal(3, 2)");

            builder.Property(e => e.ReviewDate).HasColumnType("datetime");

            builder.HasOne(d => d.User)
                .WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId);
        }
    }

}
