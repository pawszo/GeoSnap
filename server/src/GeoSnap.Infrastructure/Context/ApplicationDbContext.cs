using MongoDB.Driver;
using System.Configuration;
using GeoSnap.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using GeoSnap.Application.Interfaces;
using Microsoft.Extensions.Configuration;
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
        //var dbConnString = _configuration.GetConnectionString("postgres");
        //optionsBuilder.UseNpgsql("Data Source=(local);Initial Catalog=PwiSchwanTest;Integrated Security=True;Persist Security Info=True;Encrypt=False;");
        //optionsBuilder.UseNpgsql(dbConnString);
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