using GeoSnap.Domain.Enums;
using GeoSnap.Domain.Entities;

namespace GeoSnap.Application.Dtos;
public class NetworkAddressDto
{
    public required string IP { get; set; }
    public ProtocolVersion Version { get; set; }
    public ICollection<string> KnownDomains { get; set; } = new List<string>();
    public required NetworkAddressGeoLocationDto RecentGeoLocation { get; set; }

    public NetworkAddress MapTo()
    {
        var networkAddress = new NetworkAddress()
        {
            IP = IP,
            Version = Version,
            KnownDomains = KnownDomains
        };
        var geoLocation = new NetworkAddressGeoLocation()
        {
            CapturedAt = RecentGeoLocation.CapturedAt,
            DataProviderName = RecentGeoLocation.DataProviderName,
            Latitude = RecentGeoLocation.Latitude,
            Longitude = RecentGeoLocation.Longitude,
            ContinentCode = RecentGeoLocation.ContinentCode,
            CountryCode = RecentGeoLocation.CountryCode,
            RegionCode = RecentGeoLocation.RegionCode,
            City = RecentGeoLocation.City,
            ZipCode = RecentGeoLocation.ZipCode,
            IP = IP,
            ProtocolVersion = Version,
            NetworkAddress = networkAddress
        };
        networkAddress.GeoLocations = new[] { geoLocation }.ToList();

        return networkAddress;
    }
}