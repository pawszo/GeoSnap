﻿using GeoSnap.Domain.Enums;
using GeoSnap.Domain.Entities;

namespace GeoSnap.Application.Dtos;
public class NetworkAddressHistoryDto
{
    public required string IP { get; set; }
    public ProtocolVersion Version { get; set; }
    public string? Domain { get; set; }
    public IReadOnlyCollection<NetworkAddressGeoLocationDto> GeoLocations { get; set; } = [];

    public NetworkAddressDto Latest() => new NetworkAddressDto
    {
        IP = IP,
        Version = Version,
        Domain = Domain,
        RecentGeoLocation = GeoLocations.OrderByDescending(x => x.CapturedAt).First()
    };

    public static NetworkAddressHistoryDto MapFrom(NetworkAddress networkAddress) => new()
    {
        IP = networkAddress.IP,
        Version = networkAddress.Version,
        Domain = networkAddress.Domain,
        GeoLocations = networkAddress.GeoLocations.Select(NetworkAddressGeoLocationDto.MapFrom).ToList()
    };
}
