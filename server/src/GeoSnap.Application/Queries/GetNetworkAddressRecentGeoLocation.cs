using MediatR;
using GeoSnap.Domain.Entities;
using GeoSnap.Application.Dtos;
using GeoSnap.Domain.Extensions;
using Microsoft.Extensions.Logging;
using GeoSnap.Application.Interfaces;

namespace GeoSnap.Application.Queries;
public record class GetNetworkAddressRecentGeoLocationQuery(string networkAddress) : IRequest<NetworkAddressDto?>;
public class GetNetworkAddressRecentGeoLocationQueryHandler : IRequestHandler<GetNetworkAddressRecentGeoLocationQuery, NetworkAddressDto?>
{
    private readonly ILogger<NetworkAddress> _logger;
    private readonly IGeoLocationService _geoLocationService;
    private readonly INetworkAddressStoringService _store;

    public GetNetworkAddressRecentGeoLocationQueryHandler(
        ILogger<NetworkAddress> logger, 
        IGeoLocationService geoLocationService, 
        INetworkAddressStoringService store)
    {
        _logger = logger;
        _geoLocationService = geoLocationService;
        _store = store;
    }

    public async Task<NetworkAddressDto?> Handle(GetNetworkAddressRecentGeoLocationQuery request, CancellationToken cancellationToken)
    {
        var recentGeoLocation = await _geoLocationService.GetGeoLocationAsync(request.networkAddress);
        if (recentGeoLocation is null)
        {
            _logger.LogWarning("Provider failed to find recent geo location data for network address {networkAddress}", request.networkAddress);

            var historicData = await _store.GetHistoryAsync(request.networkAddress);
            var lastKnown = historicData?.Latest();

            if(lastKnown is not null) _logger.LogInformation("Returning last known geo location data for network address {networkAddress}", request.networkAddress);
            return lastKnown;
        }

        return await _store.SaveAsync(new NetworkAddressDto
        { 
            IP = recentGeoLocation.IP,
            Version = recentGeoLocation.ProtocolVersion,
            KnownDomains = request.networkAddress.TryGetValidDomainUrl(out var domain) ? new[] { domain } : [],
            RecentGeoLocation = recentGeoLocation
        });
    }
}