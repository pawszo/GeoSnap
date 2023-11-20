using System.Net;
using GeoSnap.Application.Interfaces;

namespace GeoSnap.Infrastructure.Services;
public class DnsResolvingService : IDnsResolvingService
{
    public async Task<string?> GetIpForDomainAsync(string networkAddress, CancellationToken cancellationToken)
    {
        var addressList = await Dns.GetHostAddressesAsync(networkAddress, cancellationToken);

        return addressList?.FirstOrDefault()?.ToString();
    }
}
