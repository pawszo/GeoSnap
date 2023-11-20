using GeoSnap.Domain.Entities;
using GeoSnap.Application.Dtos;
using GeoSnap.Domain.Extensions;
using Microsoft.Extensions.Logging;
using GeoSnap.Application.Interfaces;

namespace GeoSnap.Infrastructure.Services;
public class NetworkAddressStoringService : INetworkAddressStoringService
{
    private readonly INetworkAddressRepository _networkAddressRepository;
    private readonly ILogger<NetworkAddress> _logger;

    public NetworkAddressStoringService(INetworkAddressRepository networkAddressRepository, ILogger<NetworkAddress> logger)
    {
        _networkAddressRepository = networkAddressRepository;
        _logger = logger;
    }

    public async Task<bool> DeleteAsync(string ip, CancellationToken cancellationToken)
    {
        var record = await _networkAddressRepository.FindByIPAsync(ip, cancellationToken);
        if (record is not null)
        {
            var isDeleted = _networkAddressRepository.Delete(record);
            if(isDeleted) _logger.LogInformation("Deleted record for {ip}", ip);
            return isDeleted;
        }

        _logger.LogWarning("No record found for {ip}", ip);
        return false;
    }

    public async Task<NetworkAddressHistoryDto?> GetHistoryAsync(string ip, CancellationToken cancellationToken)
    {
        var record = await _networkAddressRepository.FindByIPAsync(ip, cancellationToken);
        if (record is not null)
        {
            return NetworkAddressHistoryDto.MapFrom(record);
        }

        _logger.LogWarning("No record found for {ip}", ip);
        return null;
    }

    public async Task<NetworkAddressDto> SaveAsync(NetworkAddressDto recent, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var current = await _networkAddressRepository.FindByIPAsync(recent.IP, cancellationToken);

        if(current is null)
        {
            var record = recent.MapTo();
            await _networkAddressRepository.AddAsync(record, cancellationToken);
            _logger.LogInformation("Created record for {IP}", recent.IP);
            return recent;
        }
        current.Domain ??= recent.Domain;
        recent.Domain ??= current.Domain;

        current.GeoLocations.Add(recent.RecentGeoLocation.MapTo(current));
        _networkAddressRepository.Update(current);

        _logger.LogInformation("Updated record for {IP}", recent.IP);
        return recent;
    }
}
