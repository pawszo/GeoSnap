using System;
using System.Linq;
using System.Text;
using GeoSnap.Domain.Enums;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GeoSnap.Application.Dtos;
public class IpStackGeoLocationDto
{
    public string Ip { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string ContinentCode { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
    public string RegionCode { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Zip { get; set; } = string.Empty;
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }

    public NetworkAddressGeoLocationDto MapTo() => new()
    {
        CapturedAt = DateTime.UtcNow,
        ProtocolVersion = Type == "ipv4" ? ProtocolVersion.IPv4 : ProtocolVersion.IPv6,
        DataProviderName = "Ipify",
        IP = Ip,
        Latitude = Latitude,
        Longitude = Longitude,
        ContinentCode = ContinentCode,
        CountryCode = CountryCode,
        RegionCode = RegionCode,
        City = City,
        ZipCode = Zip
    };
}
