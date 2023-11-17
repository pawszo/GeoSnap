using MediatR;
using GeoSnap.Domain.Entities;
using GeoSnap.Application.Dtos;
using GeoSnap.Domain.Extensions;
using Microsoft.Extensions.Logging;
using GeoSnap.Application.Interfaces;

namespace GeoSnap.Application.Queries;
public record class GetNetworkAddressRecentGeoLocationQuery(string NetworkAddress) : IRequest<NetworkAddressDto?>;
public class GetNetworkAddressRecentGeoLocationQueryHandler(
    ILogger<NetworkAddress> logger,
    IGeoLocationService geoLocationService,
    INetworkAddressStoringService store) : IRequestHandler<GetNetworkAddressRecentGeoLocationQuery, NetworkAddressDto?>
{
    public async Task<NetworkAddressDto?> Handle(GetNetworkAddressRecentGeoLocationQuery request, CancellationToken cancellationToken)
    {
        var recentGeoLocation = await geoLocationService.GetGeoLocationAsync(request.NetworkAddress, cancellationToken);
        if (recentGeoLocation is null)
        {
            logger.LogWarning("Provider failed to find recent geo location data for network address {NetworkAddress}", request.NetworkAddress);

            var historicData = await store.GetHistoryAsync(request.NetworkAddress, cancellationToken);
            var lastKnown = historicData?.Latest();

            if(lastKnown is not null) logger.LogInformation("Returning last known geo location data for network address {NetworkAddress}", request.NetworkAddress);
            return lastKnown;
        }

        return await store.SaveAsync(new NetworkAddressDto
        { 
            IP = recentGeoLocation.IP,
            Version = recentGeoLocation.ProtocolVersion,
            KnownDomains = request.NetworkAddress.TryGetValidDomainUrl(out var domain) ? new[] { domain } : [],
            RecentGeoLocation = recentGeoLocation
        }, cancellationToken);
    }
}