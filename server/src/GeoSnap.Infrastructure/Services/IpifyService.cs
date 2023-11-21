using Newtonsoft.Json;
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

    public IpifyService(IHttpClientFactory httpClientFactory, IConfiguration config, ILogger<IpifyService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _apiKey = config.GetValue<string>("ApiKey:Ipify");
    }

    public async Task<NetworkAddressGeoLocationDto?> FindIPAsync(string ip, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var client = _httpClientFactory.CreateClient("Ipify");
        var responseMessage = await client.GetAsync($"api/v1?apiKey={_apiKey}&ipAddress={ip}", cancellationToken);

        if (responseMessage.IsSuccessStatusCode)
        {
            _logger.LogInformation("Successfully retrieved data for {ip}", ip);
            var dtoString = await responseMessage.Content.ReadAsStringAsync();
            var dto = JsonConvert.DeserializeObject<IpifyNetworkAddressDto>(dtoString);

            if (dto is null)
            {
                _logger.LogError("Failed to deserialize data for {ip}", ip);
                return null;
            }

            _logger.LogInformation("Successfully deserialized data for {ip}", ip);
            return dto.MapTo();
        }

        _logger.LogError("Failed to retrieve data for {ip}", ip);
        return null;
    }
}
