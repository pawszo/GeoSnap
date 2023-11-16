using MediatR;
using GeoSnap.Application.Dtos;
using GeoSnap.Domain.Extensions;
using Microsoft.Extensions.Logging;
using GeoSnap.Application.Interfaces;

namespace GeoSnap.Application.Queries;
public record class GetNetworkAddressRecentGeoLocationQuery(string networkAddress) : IRequest<NetworkAddressDto?>;
public class GetNetworkAddressRecentGeoLocationQueryHandler : IRequestHandler<GetNetworkAddressRecentGeoLocationQuery, NetworkAddressDto?>
{
    private readonly ILogger<GetNetworkAddressRecentGeoLocationQueryHandler> _logger;
    private readonly IGeoLocationService _geoLocationService;
    private readonly INetworkAddressStoringService _store;

    public GetNetworkAddressRecentGeoLocationQueryHandler(
        ILogger<GetNetworkAddressRecentGeoLocationQueryHandler> logger, 
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
            var historicData = await _store.GetHistoryAsync(request.networkAddress);
            return historicData?.Latest() ?? null;
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