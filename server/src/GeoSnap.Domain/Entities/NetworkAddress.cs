﻿using GeoSnap.Domain.Enums;

namespace GeoSnap.Domain.Entities;
public class NetworkAddress
{
    public required string IP { get; set; }
    public required ProtocolVersion Version { get; set; }
    public ICollection<NetworkAddressGeoLocation> GeoLocations { get; set; } = new List<NetworkAddressGeoLocation>();

}