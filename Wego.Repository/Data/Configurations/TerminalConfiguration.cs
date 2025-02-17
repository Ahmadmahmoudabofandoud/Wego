using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models.Flights;

namespace Wego.Repository.Data.Configurations
{
    internal class TerminalConfiguration : IEntityTypeConfiguration<Terminal>
    {
        public void Configure(EntityTypeBuilder<Terminal> builder)
        {
            builder
               .HasOne(t => t.Airport)
               .WithMany(a => a.Terminals)
               .HasForeignKey(t => t.AirportId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
