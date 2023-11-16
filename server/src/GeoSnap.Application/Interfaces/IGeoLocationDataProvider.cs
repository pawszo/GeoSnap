using GeoSnap.Application.Dtos;

namespace GeoSnap.Application.Interfaces;
public interface IGeoLocationDataProvider
{
    Task<NetworkAddressGeoLocationDto?> FindIPV4Async(string ipV4);
    Task<NetworkAddressGeoLocationDto?> FindIPV6Async(string ipV6);
    Task<NetworkAddressGeoLocationDto?> FindDomainAsync(string domain);
}
