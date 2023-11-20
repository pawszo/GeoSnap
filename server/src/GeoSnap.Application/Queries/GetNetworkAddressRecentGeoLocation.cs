using MediatR;
using GeoSnap.Domain.Entities;
using GeoSnap.Application.Dtos;
using GeoSnap.Domain.Extensions;
using GeoSnap.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using GeoSnap.Application.Interfaces;

namespace GeoSnap.Application.Queries;
public record class GetNetworkAddressRecentGeoLocationQuery(string NetworkAddress) : IRequest<IList<NetworkAddressDto>?>;
public class GetNetworkAddressRecentGeoLocationQueryHandler(
    ILogger<NetworkAddress> logger,
    IGeoLocationService geoLocationService,
    INetworkAddressStoringService store,
    IDnsResolvingService dnsResolver) : IRequestHandler<GetNetworkAddressRecentGeoLocationQuery, IList<NetworkAddressDto>?>
{
    public async Task<IList<NetworkAddressDto>?> Handle(GetNetworkAddressRecentGeoLocationQuery request, CancellationToken cancellationToken)
    {
        if (request.NetworkAddress.TryGetValidDomainUrl(out string domainUrl))
        {
            var resolvedIps = await dnsResolver.GetIpsForDomainAsync(domainUrl, cancellationToken);
            if (resolvedIps == null || resolvedIps.Length == 0)
            {
                throw new DnsResolvingException(domainUrl);
            }

            IList<NetworkAddressDto> results = new List<NetworkAddressDto>();
            foreach (var resolvedIp in resolvedIps)
            {
                if (resolvedIp.TryGetValidIp(out string ip, out _))
                {
                    var recentGeoLocationForIp = await GetRecentGeoLocationForIp(resolvedIp, cancellationToken, domainUrl);
                    if (recentGeoLocationForIp is not null) results.Add(recentGeoLocationForIp);
                }
            }
            return results.Count > 0 ? results : null;
        }

        request.NetworkAddress.TryGetValidIp(out var validIp, out _);
        var recentGeoLocation = await GetRecentGeoLocationForIp(validIp, cancellationToken);

        return recentGeoLocation is null ? null : [recentGeoLocation];
    }

    private async Task<NetworkAddressDto?> GetRecentGeoLocationForIp(string ip, CancellationToken cancellationToken, string domainUrl = "")
    {
        var recentGeoLocation = await geoLocationService.GetGeoLocationAsync(ip, cancellationToken);
        if (recentGeoLocation is null)
        {
            logger.LogWarning("Provider failed to find recent geo location data for ip {ip}", ip);

            var historicData = await store.GetHistoryAsync(ip, cancellationToken);
            var lastKnown = historicData?.Latest();
            if (lastKnown is not null)
            {
                logger.LogInformation("Returning last known geo location data for ip {ip}", ip);
            }
            return lastKnown;
        }

        return await store.SaveAsync(new NetworkAddressDto
        {
            IP = recentGeoLocation.IP,
            Version = recentGeoLocation.ProtocolVersion,
            RecentGeoLocation = recentGeoLocation,
            Domain = domainUrl
        }, cancellationToken);
    }
}