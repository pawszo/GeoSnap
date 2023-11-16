﻿namespace GeoSnap.Domain.Entities;
public class NetworkAddressGeoLocation
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
    public required NetworkAddress NetworkAddress { get; set; }
}