using MediatR;
using GeoSnap.Domain.Entities;
using GeoSnap.Application.Dtos;
using Microsoft.Extensions.Logging;
using GeoSnap.Application.Interfaces;

namespace GeoSnap.Application.Queries;
public record class GetNetworkAddressHistoricGeoLocationsQuery(string NetworkAddress) : IRequest<NetworkAddressHistoryDto?>;

public class GetNetworkAddressHistoricGeoLocationsQueryHandler(
    ILogger<NetworkAddress> logger, 
    INetworkAddressStoringService store) : IRequestHandler<GetNetworkAddressHistoricGeoLocationsQuery, NetworkAddressHistoryDto?>
{
    public async Task<NetworkAddressHistoryDto?> Handle(GetNetworkAddressHistoricGeoLocationsQuery request, CancellationToken cancellationToken)
    {
        var result = await store.GetHistoryAsync(request.NetworkAddress, cancellationToken);

        if(result is null)
        {
            logger.LogWarning("No records were found for {NetworkAddress}", request.NetworkAddress);
        }

        return result;
    }
}
