using GeoSnap.Domain.Entities;

namespace GeoSnap.Application.Interfaces;
public interface INetworkAddressRepository
{
    Task<IQueryable<NetworkAddress>> QueryAllAsync();
    Task<NetworkAddress> FindByIPAsync(string ip);
    Task<NetworkAddress> FindByDomainUrlAsync(string domainUrl);
    Task AddAsync(NetworkAddress record);
    Task UpdateAsync(NetworkAddress record);
    Task DeleteAsync(NetworkAddress record);
}
