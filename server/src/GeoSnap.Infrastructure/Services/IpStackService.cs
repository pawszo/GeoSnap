using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoSnap.Application.Dtos;
using System.Collections.Generic;
using GeoSnap.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace GeoSnap.Infrastructure.Services;
public class IpStackService : IGeoLocationDataProvider
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _apiKey;

    public IpStackService(IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        _httpClientFactory = httpClientFactory;
        _apiKey = config.GetValue<string>("ApiKey:IpStack");
    }

    public Task<NetworkAddressGeoLocationDto?> FindDomainAsync(string domain, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        throw new NotImplementedException();
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
