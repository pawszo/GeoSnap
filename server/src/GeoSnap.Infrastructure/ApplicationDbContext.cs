using GeoSnap.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GeoSnap.Infrastructure;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<NetworkAddress> NetworkAddresses => Set<NetworkAddress>();
    public DbSet<NetworkAddressGeoLocation> GeoLocations => Set<NetworkAddressGeoLocation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NetworkAddressGeoLocation>()
            .HasKey(g => new { g.IP, g.CapturedAt });
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