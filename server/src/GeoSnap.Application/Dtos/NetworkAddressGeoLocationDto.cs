using GeoSnap.Domain.Enums;
using GeoSnap.Domain.Entities;

namespace GeoSnap.Application.Dtos;
public class NetworkAddressGeoLocationDto
{
    public DateTime CapturedAt { get; set; }
    public required string DataProviderName { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public required string ContinentCode { get; set; }
    public required string CountryCode { get; set; }
    public required string RegionCode { get; set; }
    public required string City { get; set; }
    public required string ZipCode { get; set; }

    // Foreign key to NetworkAddress
    public required string IP { get; set; }
    public ProtocolVersion ProtocolVersion { get; set; }

    public static NetworkAddressGeoLocationDto MapFrom(NetworkAddressGeoLocation networkAddressGeoLocation) => new()
    {
        CapturedAt = networkAddressGeoLocation.CapturedAt,
        DataProviderName = networkAddressGeoLocation.DataProviderName,
        Latitude = networkAddressGeoLocation.Latitude,
        Longitude = networkAddressGeoLocation.Longitude,
        ContinentCode = networkAddressGeoLocation.ContinentCode,
        CountryCode = networkAddressGeoLocation.CountryCode,
        RegionCode = networkAddressGeoLocation.RegionCode,
        City = networkAddressGeoLocation.City,
        ZipCode = networkAddressGeoLocation.ZipCode,
        IP = networkAddressGeoLocation.IP,
        ProtocolVersion = networkAddressGeoLocation.ProtocolVersion,
    };

    public NetworkAddressGeoLocation MapTo(NetworkAddress networkAddress) => new()
    {
        CapturedAt = CapturedAt,
        DataProviderName = DataProviderName,
        Latitude = Latitude,
        Longitude = Longitude,
        ContinentCode = ContinentCode,
        CountryCode = CountryCode,
        RegionCode = RegionCode,
        City = City,
        ZipCode = ZipCode,
        IP = IP,
        ProtocolVersion = ProtocolVersion,
        NetworkAddress = networkAddress
    };
}
