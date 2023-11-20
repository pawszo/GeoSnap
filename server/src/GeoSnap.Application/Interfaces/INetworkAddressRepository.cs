using GeoSnap.Domain.Entities;

namespace GeoSnap.Application.Interfaces;
public interface INetworkAddressRepository
{
    Task<IReadOnlyCollection<NetworkAddress>> GetAllAsync(Func<NetworkAddress, bool> filter, CancellationToken cancellationToken);
    Task<NetworkAddress?> FindByIPAsync(string ip, CancellationToken cancellationToken);
    Task<NetworkAddress> AddAsync(NetworkAddress record, CancellationToken cancellationToken);
    NetworkAddress Update(NetworkAddress record);
    bool Delete(NetworkAddress record);
}
