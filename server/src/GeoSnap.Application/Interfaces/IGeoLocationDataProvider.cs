using GeoSnap.Application.Dtos;

namespace GeoSnap.Application.Interfaces;
public interface IGeoLocationDataProvider
{
    Task<NetworkAddressGeoLocationDto?> FindGeoLocationAsync(string ip);
}
