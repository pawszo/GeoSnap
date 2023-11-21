using GeoSnap.Application.Dtos;

namespace GeoSnap.Application.Interfaces;
public interface IGeoLocationDataProvider
{
    Task<NetworkAddressGeoLocationDto?> FindIPAsync(string ipAddress, CancellationToken cancellationToken);
}
