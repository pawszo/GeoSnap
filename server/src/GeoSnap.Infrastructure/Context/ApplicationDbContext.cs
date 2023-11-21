using GeoSnap.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using GeoSnap.Application.Interfaces;

namespace GeoSnap.Infrastructure.Context;
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<NetworkAddress> NetworkAddresses { get; set; }
    public DbSet<NetworkAddressGeoLocation> GeoLocations { get; set; }

}