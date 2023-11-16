using GeoSnap.Domain.Entities;
using System.Collections.Generic;

namespace GeoSnap.Application.Interfaces;
public interface INetworkAddressRepository
{
    Task<IReadOnlyCollection<NetworkAddress>> GetAllAsync(Func<NetworkAddress, bool> filter);
    Task<NetworkAddress?> FindByIPAsync(string ip);
    Task<NetworkAddress?> FindByDomainUrlAsync(string domainUrl);
    Task AddAsync(NetworkAddress record);
    Task UpdateAsync(NetworkAddress record);
    Task DeleteAsync(NetworkAddress record);
}
