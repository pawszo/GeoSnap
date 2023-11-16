using GeoSnap.Domain.Enums;

namespace GeoSnap.Application.Dtos;
public class NetworkAddressDto
{
    public required string IP { get; set; }
    public ProtocolVersion Version { get; set; }
    public ICollection<string> KnownDomains { get; set; } = new List<string>();
    public NetworkAddressGeoLocationDto? RecentGeoLocation { get; set; }
}