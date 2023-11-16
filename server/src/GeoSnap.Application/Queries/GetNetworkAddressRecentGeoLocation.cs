using MediatR;
using GeoSnap.Application.Dtos;
using Microsoft.Extensions.Logging;
using GeoSnap.Application.Interfaces;

namespace GeoSnap.Application.Queries;
public record class GetNetworkAddressRecentGeoLocationQuery(string IP) : IRequest<NetworkAddressDto?>;
public class GetNetworkAddressRecentGeoLocationQueryHandler : IRequestHandler<GetNetworkAddressRecentGeoLocationQuery, NetworkAddressDto?>
{
    private readonly ILogger<GetNetworkAddressRecentGeoLocationQueryHandler> _logger;
    private readonly IGeoLocationService _geoLocationService;
    private readonly IDbService _dbService;

    public GetNetworkAddressRecentGeoLocationQueryHandler(
        ILogger<GetNetworkAddressRecentGeoLocationQueryHandler> logger, 
        IGeoLocationService geoLocationService, 
        IDbService dbService)
    {
        _logger = logger;
        _geoLocationService = geoLocationService;
        _dbService = dbService;
    }

    public Task<NetworkAddressDto?> Handle(GetNetworkAddressRecentGeoLocationQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}