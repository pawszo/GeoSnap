using GeoSnap.Application.Dtos;

namespace GeoSnap.Application.Interfaces;
public interface IGeoLocationDataProvider
{
    Task<NetworkAddressGeoLocationDto?> FindIPV4Async(string ipV4, CancellationToken cancellationToken);
    Task<NetworkAddressGeoLocationDto?> FindIPV6Async(string ipV6, CancellationToken cancellationToken);
    Task<NetworkAddressGeoLocationDto?> FindDomainAsync(string domain, CancellationToken cancellationToken);
}
