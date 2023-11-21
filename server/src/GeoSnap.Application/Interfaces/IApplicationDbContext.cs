using GeoSnap.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GeoSnap.Application.Interfaces;
public interface IApplicationDbContext
{
    DbSet<NetworkAddress> NetworkAddresses { get; }
    DbSet<NetworkAddressGeoLocation> GeoLocations { get; }
}