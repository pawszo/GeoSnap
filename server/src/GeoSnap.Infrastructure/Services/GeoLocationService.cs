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
    public async Task<NetworkAddressGeoLocationDto?> GetGeoLocationAsync(string networkAddress)
    {
        NetworkAddressGeoLocationDto? geoLocationData = null;

        if (networkAddress.TryGetValidIp(out var ip, out var protocol))
        {
            geoLocationData = protocol == Domain.Enums.ProtocolVersion.IPv4 
                ? await mainProvider.FindIPV4Async(ip)
                : await mainProvider.FindIPV6Async(ip);

            geoLocationData ??= protocol == Domain.Enums.ProtocolVersion.IPv4 
                ? await alternativeProvider.FindIPV4Async(ip)
                : await alternativeProvider.FindIPV6Async(ip);
        }
        else if(networkAddress.TryGetValidDomainUrl(out var domain))
        {
            geoLocationData = await mainProvider.FindDomainAsync(domain);
            geoLocationData ??= await alternativeProvider.FindDomainAsync(domain);
        }

        return geoLocationData;
    }
}