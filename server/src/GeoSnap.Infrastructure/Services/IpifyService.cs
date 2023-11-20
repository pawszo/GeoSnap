using GeoSnap.Application.Dtos;
using Microsoft.Extensions.Logging;
using GeoSnap.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace GeoSnap.Infrastructure.Services;
public class IpifyService : IGeoLocationDataProvider
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<IpifyService> _logger;
    private readonly string _apiKey;
    private readonly string _baseUrlV4;
    private readonly string _baseUrlV6;

    public IpifyService(IHttpClientFactory httpClientFactory, IConfiguration config, ILogger<IpifyService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _apiKey = config.GetValue<string>("ApiKey:Ipify");
        _baseUrlV4 = config.GetValue<string>("BaseUrl:IpifyV4");
        _baseUrlV6 = config.GetValue<string>("BaseUrl:IpifyV6");
    }

    public Task<NetworkAddressGeoLocationDto?> FindIPV4Async(string ipV4, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        throw new NotImplementedException();
    }

    public Task<NetworkAddressGeoLocationDto?> FindIPV6Async(string ipV6, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        throw new NotImplementedException();
    }
}
