using GeoSnap.Domain.Entities;

namespace GeoSnap.Application.Interfaces;
public interface INetworkAddressRepository
{
    Task<IReadOnlyCollection<NetworkAddress>> GetAllAsync(Func<NetworkAddress, bool> filter, CancellationToken cancellationToken);
    Task<NetworkAddress?> FindByIPAsync(string ip, CancellationToken cancellationToken);
    Task<NetworkAddress?> FindByDomainUrlAsync(string domainUrl, CancellationToken cancellationToken);
    Task AddAsync(NetworkAddress record, CancellationToken cancellationToken);
    void Update(NetworkAddress record);
    void Delete(NetworkAddress record);
}
