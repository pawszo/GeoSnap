using System;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using GeoSnap.Application.Dtos;
using System.Collections.Generic;
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
        return await FindAsync(domain);
    }

    public async Task<NetworkAddressGeoLocationDto?> FindIPV4Async(string ipV4, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await FindAsync(ipV4);
    }

    public async Task<NetworkAddressGeoLocationDto?> FindIPV6Async(string ipV6, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await FindAsync(ipV6);
    }

    private async Task<NetworkAddressGeoLocationDto?> FindAsync(string networkAddress)
    {
        var client = _httpClientFactory.CreateClient("IpStack");
        var responseMessage = await client.GetAsync($"{networkAddress}?access_key={_apiKey}&output=json");

        if(responseMessage.IsSuccessStatusCode)
        {
            _logger.LogInformation("Successfully retrieved data for {networkAddress}", networkAddress);
            using var contentStream = await responseMessage.Content.ReadAsStreamAsync();
            var dto = await JsonSerializer.DeserializeAsync<IpStackGeoLocationDto>(contentStream);

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
