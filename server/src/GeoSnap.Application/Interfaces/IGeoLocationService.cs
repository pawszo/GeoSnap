using GeoSnap.Application.Dtos;

namespace GeoSnap.Application.Interfaces;
public interface IGeoLocationService
{
    Task<NetworkAddressGeoLocationDto?> GetGeoLocationAsync(string networkAddress);
}
