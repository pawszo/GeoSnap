using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoSnap.Domain.Entities;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using GeoSnap.Infrastructure.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeoSnap.Infrastructure.Data.Configurations;
public class NetworkAddressConfigurator : BaseContextConfiguration<NetworkAddress>
{
    public NetworkAddressConfigurator(EntityTypeBuilder<NetworkAddress> builder) : base(builder)
    {
    }

    public override void Configure()
    {
        Builder.Property(n => n.IP)
            .IsRequired();
        Builder.Property(n => n.Domain)
            .IsRequired(false);
        Builder.Property(n => n.Version)
            .HasConversion<string>();
    }   
}
