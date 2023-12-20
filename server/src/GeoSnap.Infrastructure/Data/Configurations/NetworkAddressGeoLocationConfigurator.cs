using GeoSnap.Domain.Entities;
using GeoSnap.Infrastructure.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeoSnap.Infrastructure.Data.Configurations;
public class NetworkAddressGeoLocationConfigurator : BaseContextConfiguration<NetworkAddressGeoLocation>
{
    public NetworkAddressGeoLocationConfigurator(EntityTypeBuilder<NetworkAddressGeoLocation> builder) : base(builder)
    {
    }

    public override void Configure()
    {
        Builder.Property(g => g.IP)
            .IsRequired();
        Builder.HasKey(g => new { g.IP, g.CapturedAt });
        Builder.HasOne(g => g.NetworkAddress)
            .WithMany(n => n.GeoLocations)
            .HasForeignKey(g => g.IP);
        Builder.Property(g => g.ProtocolVersion)
            .HasConversion<string>();
    }
}
