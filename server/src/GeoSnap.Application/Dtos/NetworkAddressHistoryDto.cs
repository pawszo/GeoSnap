using GeoSnap.Domain.Enums;

namespace GeoSnap.Application.Dtos;
public class NetworkAddressHistoryDto
{
    public required string IP { get; set; }
    public ProtocolVersion Version { get; set; }
    public IReadOnlyCollection<NetworkAddressGeoLocationDto> GeoLocations { get; set; } = [];
}
