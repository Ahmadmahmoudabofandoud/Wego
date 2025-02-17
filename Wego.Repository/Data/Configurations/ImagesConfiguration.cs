using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models.Room;

namespace Wego.Repository.Data.Configurations
{
    internal class ImagesConfiguration : IEntityTypeConfiguration<Images>
    {
        public void Configure(EntityTypeBuilder<Images> builder)
        {
            builder
             .HasOne(i => i.Room)
             .WithMany(r => r.Images)
             .HasForeignKey(i => i.RoomId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
