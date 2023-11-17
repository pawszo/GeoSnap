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

    public async Task DeleteAsync(string networkAddress)
    {
        var record = await GetByNetworkAddress(networkAddress);
        if (record is not null)
        {
            await _networkAddressRepository.DeleteAsync(record);
            _logger.LogInformation("Deleted record for {networkAddress}", networkAddress);
            return;
        }

        _logger.LogWarning("No record found for {networkAddress}", networkAddress);
    }

    public async Task<NetworkAddressHistoryDto?> GetHistoryAsync(string networkAddress)
    {
        var record = await GetByNetworkAddress(networkAddress);
        if (record is not null)
        {
            return NetworkAddressHistoryDto.MapFrom(record);
        }

        _logger.LogWarning("No record found for {networkAddress}", networkAddress);
        return null;
    }

    public async Task<NetworkAddressDto> SaveAsync(NetworkAddressDto recent)
    {
        var current = await _networkAddressRepository.FindByIPAsync(recent.IP);

        if(current is null)
        {
            var record = recent.MapTo();
            await _networkAddressRepository.AddAsync(record);
            _logger.LogInformation("Created record for {IP}", recent.IP);
            return recent;
        }

        current.KnownDomains = Enumerable.Concat( current.KnownDomains, recent.KnownDomains).Distinct().ToArray();
        current.GeoLocations.Add(recent.RecentGeoLocation.MapTo(current));
        await _networkAddressRepository.UpdateAsync(current);

        _logger.LogInformation("Updated record for {IP}", recent.IP);
        return NetworkAddressHistoryDto.MapFrom(current).Latest();
    }

    private async Task<NetworkAddress?> GetByNetworkAddress(string networkAddress)
    {
        return networkAddress.TryGetValidDomainUrl(out string domainUrl)
            ? await _networkAddressRepository.FindByDomainUrlAsync(domainUrl)
                : networkAddress.TryGetValidIp(out string ip, out _)
                    ? await _networkAddressRepository.FindByIPAsync(ip) : null;
    }   
}
