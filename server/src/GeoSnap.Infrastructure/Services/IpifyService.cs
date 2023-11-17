using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoSnap.Application.Dtos;
using System.Collections.Generic;
using GeoSnap.Application.Interfaces;

namespace GeoSnap.Infrastructure.Services;
public class IpifyService : IGeoLocationDataProvider
{
    public Task<NetworkAddressGeoLocationDto?> FindDomainAsync(string domain)
    {
        throw new NotImplementedException();
    }

    public Task<NetworkAddressGeoLocationDto?> FindIPV4Async(string ipV4)
    {
        throw new NotImplementedException();
    }

    public Task<NetworkAddressGeoLocationDto?> FindIPV6Async(string ipV6)
    {
        throw new NotImplementedException();
    }
}
