using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoSnap.Domain.Entities;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeoSnap.Infrastructure.Data.Configurations;
public class NetworkAddressConfiguration : IEntityTypeConfiguration<NetworkAddress>
{
    public void Configure(EntityTypeBuilder<NetworkAddress> builder)
    {
        builder.Property(n => n.IP)
            .IsRequired();
        builder.Property(n => n.Domain)
            .IsRequired(false);
        builder.Property(n => n.Version)
            .HasConversion<string>();
    }   
}
