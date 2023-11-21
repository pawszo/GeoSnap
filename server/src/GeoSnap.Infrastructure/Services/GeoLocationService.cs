using GeoSnap.Application.Dtos;
using GeoSnap.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GeoSnap.Infrastructure.Services;
public class GeoLocationService(
    [FromKeyedServices("main")] IGeoLocationDataProvider mainProvider, 
    [FromKeyedServices("alternative")] IGeoLocationDataProvider alternativeProvider
    ) : IGeoLocationService
{
    public async Task<NetworkAddressGeoLocationDto?> GetGeoLocationAsync(string ipAddress, CancellationToken cancellationToken)
    {
        NetworkAddressGeoLocationDto? geoLocationData = await mainProvider.FindIPAsync(ipAddress, cancellationToken);

        return geoLocationData ?? await alternativeProvider.FindIPAsync(ipAddress, cancellationToken);
    }
}