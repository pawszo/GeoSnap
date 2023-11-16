using GeoSnap.Domain.Enums;

namespace GeoSnap.Application.Dtos;
public class NetworkAddressGeoLocationDto
{
    public DateTime? CapturedAt { get; set; }
    public string? DataProviderName { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string? ContinentCode { get; set; }
    public string? CountryCode { get; set; }
    public string? RegionCode { get; set; }
    public string? City { get; set; }
    public string? ZipCode { get; set; }

    // Foreign key to NetworkAddress
    public required string IP { get; set; }
    public ProtocolVersion ProtocolVersion { get; set; }
}
