using GeoSnap.Domain.Enums;
using GeoSnap.Domain.Entities;
using GeoSnap.Application.Dtos;

namespace GeoSnap.Infrastructure.Tests.TestData;
public static class TestDataBuilder
{
    public static NetworkAddressDto CreateNetworkAddressDto(string ip, string? domain, ProtocolVersion ipVersion, NetworkAddressGeoLocationDto recentGeoLocation = null)
    {
        return new NetworkAddressDto
        {
            IP = ip,
            Version = ipVersion,
            Domain = domain,
            RecentGeoLocation = recentGeoLocation ?? CreateGeoLocationDto(ip, DateTime.UtcNow, ipVersion: ipVersion)
        };
    }

    public static NetworkAddressGeoLocationDto CreateGeoLocationDto(string ip, DateTime capturedAt, string dataProviderName = "manual", string countryCode = "", string city = "", string regionCode = "", string zipCode = "", 
        string continentCode = "", ProtocolVersion ipVersion = ProtocolVersion.IPv4, decimal latitude = 0, decimal longitude = 0) => 
        new()
        {
            IP = ip,
            ProtocolVersion = ipVersion,
            ContinentCode = continentCode,
            CountryCode = countryCode,
            City = city,
            RegionCode = regionCode,
            ZipCode = zipCode,
            CapturedAt = capturedAt,
            DataProviderName = dataProviderName,
            Latitude = latitude,
            Longitude = longitude
        };

    public static NetworkAddress CreateNetworkAddress(string ip, string? domain, ProtocolVersion ipVersion, NetworkAddressGeoLocationDto recentGeoLocation = null, IEnumerable<NetworkAddressGeoLocation> geoLocations = null)
    {
        var record = new NetworkAddress
        {
            IP = ip,
            Version = ipVersion,
            Domain = domain,
            GeoLocations = geoLocations?.ToList() ?? new List<NetworkAddressGeoLocation>()
        };
        if(geoLocations is not null)
        {
            foreach(var geoLocation in geoLocations)
            {
                geoLocation.NetworkAddress = record;
                record.GeoLocations.Add(geoLocation);
            }
            return record;
        }

        var geoLocationRecord = CreateGeoLocation(ip, DateTime.UtcNow, record, ipVersion: ipVersion);
        record.GeoLocations.Add(geoLocationRecord);

        return record;
    }

    public static NetworkAddressGeoLocation CreateGeoLocation(string ip, DateTime capturedAt, NetworkAddress parent, string dataProviderName = "manual", string countryCode = "", string city = "", string regionCode = "", string zipCode = "",
        string continentCode = "", ProtocolVersion ipVersion = ProtocolVersion.IPv4, decimal latitude = 0, decimal longitude = 0) =>
        new()
        {
            IP = ip,
            ProtocolVersion = ipVersion,
            ContinentCode = continentCode,
            CountryCode = countryCode,
            City = city,
            RegionCode = regionCode,
            ZipCode = zipCode,
            CapturedAt = capturedAt,
            DataProviderName = dataProviderName,
            Latitude = latitude,
            Longitude = longitude,
            NetworkAddress = parent
        };

}
