using MediatR;
using GeoSnap.Infrastructure;
using GeoSnap.Application.Dtos;
using Microsoft.Extensions.Logging;
using GeoSnap.Application.Interfaces;

namespace GeoSnap.Application.Queries;
public record class GetNetworkAddressRecentGeoLocationQuery(string IP) : IRequest<NetworkAddressDto?>;
public class GetNetworkAddressRecentGeoLocationQueryHandler : IRequestHandler<GetNetworkAddressRecentGeoLocationQuery, NetworkAddressDto?>
{
    private readonly ILogger<GetNetworkAddressRecentGeoLocationQueryHandler> _logger;
    private readonly IGeoLocationDataProvider _geoLocationDataProvider;
    private readonly ApplicationDbContext _dbContext;

    public GetNetworkAddressRecentGeoLocationQueryHandler(ILogger<GetNetworkAddressRecentGeoLocationQueryHandler> logger, IGeoLocationDataProvider geoLocationDataProvider, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _geoLocationDataProvider = geoLocationDataProvider;
        _dbContext = dbContext;
    }

    public Task<NetworkAddressDto?> Handle(GetNetworkAddressRecentGeoLocationQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}