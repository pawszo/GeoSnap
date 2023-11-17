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

    public async Task<bool> DeleteAsync(string networkAddress, CancellationToken cancellationToken)
    {
        var record = await GetByNetworkAddress(networkAddress, cancellationToken);
        if (record is not null)
        {
            await _networkAddressRepository.DeleteAsync(record, cancellationToken);
            _logger.LogInformation("Deleted record for {networkAddress}", networkAddress);
            return true;
        }

        _logger.LogWarning("No record found for {networkAddress}", networkAddress);
        return false;
    }

    public async Task<NetworkAddressHistoryDto?> GetHistoryAsync(string networkAddress, CancellationToken cancellationToken)
    {
        var record = await GetByNetworkAddress(networkAddress, cancellationToken);
        if (record is not null)
        {
            return NetworkAddressHistoryDto.MapFrom(record);
        }

        _logger.LogWarning("No record found for {networkAddress}", networkAddress);
        return null;
    }

    public async Task<NetworkAddressDto> SaveAsync(NetworkAddressDto recent, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var current = await _networkAddressRepository.FindByIPAsync(recent.IP, cancellationToken);

        if(current is null)
        {
            var record = recent.MapTo();
            _networkAddressRepository.AddAsync(record, cancellationToken);
            _logger.LogInformation("Created record for {IP}", recent.IP);
            return recent;
        }

        current.KnownDomains = Enumerable.Concat( current.KnownDomains, recent.KnownDomains).Distinct().ToArray();
        current.GeoLocations.Add(recent.RecentGeoLocation.MapTo(current));
        await _networkAddressRepository.UpdateAsync(current, cancellationToken);

        _logger.LogInformation("Updated record for {IP}", recent.IP);
        return NetworkAddressHistoryDto.MapFrom(current).Latest();
    }

    private async Task<NetworkAddress?> GetByNetworkAddress(string networkAddress, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return networkAddress.TryGetValidDomainUrl(out string domainUrl)
            ? await _networkAddressRepository.FindByDomainUrlAsync(domainUrl, cancellationToken)
                : networkAddress.TryGetValidIp(out string ip, out _)
                    ? await _networkAddressRepository.FindByIPAsync(ip, cancellationToken) : null;
    }   
}
