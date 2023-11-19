using Newtonsoft.Json;
using System.Text.Json;
using System.Net.Http.Json;
using GeoSnap.Application.Dtos;
using Microsoft.Extensions.Logging;
using GeoSnap.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace GeoSnap.Infrastructure.Services;
public class IpStackService : IGeoLocationDataProvider
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<IpStackService> _logger;
    private readonly string _apiKey;

    public IpStackService(IHttpClientFactory httpClientFactory, IConfiguration config, ILogger<IpStackService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _apiKey = config.GetValue<string>("ApiKey:IpStack");
    }

    public async Task<NetworkAddressGeoLocationDto?> FindDomainAsync(string domain, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await FindAsync(domain, cancellationToken);
    }

    public async Task<NetworkAddressGeoLocationDto?> FindIPV4Async(string ipV4, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await FindAsync(ipV4, cancellationToken);
    }

    public async Task<NetworkAddressGeoLocationDto?> FindIPV6Async(string ipV6, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await FindAsync(ipV6, cancellationToken);
    }

    private async Task<NetworkAddressGeoLocationDto?> FindAsync(string networkAddress, CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient("IpStack");
        var responseMessage = await client.GetAsync($"{networkAddress}?access_key={_apiKey}&output=json");

        if(responseMessage.IsSuccessStatusCode)
        {
            _logger.LogInformation("Successfully retrieved data for {networkAddress}", networkAddress);
            var dtoString = await responseMessage.Content.ReadAsStringAsync();
            var dto = JsonConvert.DeserializeObject<IpStackGeoLocationDto>(dtoString);

            if(dto is null)
            {
                _logger.LogError("Failed to deserialize data for {networkAddress}", networkAddress);
                return null;
            }

            _logger.LogInformation("Successfully deserialized data for {networkAddress}", networkAddress);
            return dto.MapTo();
        }

        _logger.LogError("Failed to retrieve data for {networkAddress}", networkAddress);
        return null;
    }
}
