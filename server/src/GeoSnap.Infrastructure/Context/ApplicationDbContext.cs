using MongoDB.Driver;
using GeoSnap.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using GeoSnap.Application.Interfaces;
using GeoSnap.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeoSnap.Infrastructure.Context;
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public ApplicationDbContext() : base()
    {
    }

    public DbSet<NetworkAddress> NetworkAddresses => Set<NetworkAddress>();
    public DbSet<NetworkAddressGeoLocation> GeoLocations => Set<NetworkAddressGeoLocation>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        //// This line is required for migrations only. Uncomment and adjust the connection string for the migration and comment it again afterwards.
        //optionsBuilder.UseSqlServer("Data Source=(local);Initial Catalog=PwiSchwanTest;Integrated Security=True;Persist Security Info=True;Encrypt=False;");
        ////
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.HasDefaultSchema("pwi");
        //modelBuilder.HasSequence<long>(SequenceConstants.IdocCounterSequenceName, "pwi")
        //            .StartsAt(SequenceConstants.SequenceInitialValue)
        //            .IncrementsBy(1);
        new NetworkAddressConfigurator(modelBuilder.Entity<NetworkAddress>()).Configure();
        new NetworkAddressGeoLocationConfigurator(modelBuilder.Entity<NetworkAddressGeoLocation>()).Configure();
    }

}

public interface IContextConfiguration
{
    public void Configure();
}
public abstract class BaseContextConfiguration<T> : IContextConfiguration where T : DomainObject
{
    protected readonly EntityTypeBuilder<T> Builder;

    protected BaseContextConfiguration(EntityTypeBuilder<T> builder)
    {
        Builder = builder;
    }
    public abstract void Configure();
}