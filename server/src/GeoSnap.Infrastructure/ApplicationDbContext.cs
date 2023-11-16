using GeoSnap.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GeoSnap.Infrastructure;
public class ApplicationDbContext : DbContext
{
    public DbSet<NetworkAddress> NetworkAddresses { get; set; }
    public DbSet<NetworkAddressGeoLocation> GeoLocations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NetworkAddressGeoLocation>()
            .HasOne(g => g.NetworkAddress)
            .WithMany(n => n.GeoLocations)
            .HasForeignKey(g => g.IP);

        modelBuilder.Entity<NetworkAddress>()
            .Property(n => n.Version)
            .HasConversion<string>();
        modelBuilder.Entity<NetworkAddress>()
            .HasKey(n => n.IP);
    }
}