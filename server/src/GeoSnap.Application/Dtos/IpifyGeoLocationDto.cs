using Newtonsoft.Json;
using GeoSnap.Domain.Extensions;
using Newtonsoft.Json.Serialization;

namespace GeoSnap.Application.Dtos;
[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Ignore)]
public class IpifyNetworkAddressDto
{
    public string Ip { get; set; } = string.Empty;
    public IpifyLocationDto Location { get; set; }

    public NetworkAddressGeoLocationDto MapTo()
    {
        Ip.TryGetValidIp(out _, out var version);

        NetworkAddressGeoLocationDto networkAddressGeoLocationDto = new()
        {
            CapturedAt = DateTime.UtcNow,
            ProtocolVersion = version,
            DataProviderName = "Ipify",
            IP = Ip,
            Latitude = Location.Lat,
            Longitude = Location.Lng,
            ContinentCode = "",
            CountryCode = Location.Country,
            RegionCode = Location.Region,
            City = Location.City,
            ZipCode = Location.PostalCode
        };
        return networkAddressGeoLocationDto;
    }
}

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Ignore)]
public class IpifyLocationDto
{
    public decimal Lat { get; set; }
    public decimal Lng { get; set; }
    public string Country { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
}