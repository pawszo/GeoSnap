using GeoSnap.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeoSnap.Infrastructure.Data.Configurations;
public class NetworkAddressGeoLocationConfiguration : IEntityTypeConfiguration<NetworkAddressGeoLocation>
{
    public void Configure(EntityTypeBuilder<NetworkAddressGeoLocation> builder)
    {
        builder.Property(g => g.IP)
            .IsRequired();
        builder.HasKey(g => new { g.IP, g.CapturedAt });
        builder.HasOne(g => g.NetworkAddress)
            .WithMany(n => n.GeoLocations)
            .HasForeignKey(g => g.IP);
        builder.Property(g => g.ProtocolVersion)
            .HasConversion<string>();
    }
}
