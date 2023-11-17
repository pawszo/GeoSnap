using GeoSnap.Application.Dtos;
using GeoSnap.Domain.Extensions;
using GeoSnap.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GeoSnap.Infrastructure.Services;
public class GeoLocationService(
    [FromKeyedServices("main")] IGeoLocationDataProvider mainProvider, 
    [FromKeyedServices("alternative")] IGeoLocationDataProvider alternativeProvider
    ) : IGeoLocationService
{
    public async Task<NetworkAddressGeoLocationDto?> GetGeoLocationAsync(string networkAddress, CancellationToken cancellationToken)
    {
        NetworkAddressGeoLocationDto? geoLocationData = null;

        if (networkAddress.TryGetValidIp(out var ip, out var protocol))
        {
            geoLocationData = protocol == Domain.Enums.ProtocolVersion.IPv4 
                ? await mainProvider.FindIPV4Async(ip, cancellationToken)
                : await mainProvider.FindIPV6Async(ip, cancellationToken);

            geoLocationData ??= protocol == Domain.Enums.ProtocolVersion.IPv4 
                ? await alternativeProvider.FindIPV4Async(ip, cancellationToken)
                : await alternativeProvider.FindIPV6Async(ip, cancellationToken);
        }
        else if(networkAddress.TryGetValidDomainUrl(out var domain))
        {
            geoLocationData = await mainProvider.FindDomainAsync(domain, cancellationToken);
            geoLocationData ??= await alternativeProvider.FindDomainAsync(domain, cancellationToken);
        }

        return geoLocationData;
    }
}