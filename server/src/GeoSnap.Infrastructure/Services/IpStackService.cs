using Newtonsoft.Json;
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

    public async Task<NetworkAddressGeoLocationDto?> FindIPAsync(string ip, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var client = _httpClientFactory.CreateClient("IpStack");
        var responseMessage = await client.GetAsync($"{ip}?access_key={_apiKey}&output=json", cancellationToken);

        if (responseMessage.IsSuccessStatusCode)
        {
            _logger.LogInformation("Successfully retrieved data for {ip}", ip);
            var dtoString = await responseMessage.Content.ReadAsStringAsync();
            var dto = JsonConvert.DeserializeObject<IpStackGeoLocationDto>(dtoString);

            if (dto is null)
            {
                _logger.LogError("Failed to deserialize data for {ip}", ip);
                return null;
            }

            _logger.LogInformation("Successfully deserialized data for {ipAddress}", ip);
            return dto.MapTo();
        }

        _logger.LogError("Failed to retrieve data for {ip}", ip);
        return null;
    }
}
