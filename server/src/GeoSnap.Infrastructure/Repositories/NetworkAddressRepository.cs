using GeoSnap.Domain.Entities;
using GeoSnap.Domain.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using GeoSnap.Application.Interfaces;

namespace GeoSnap.Infrastructure.Repositories;
public class NetworkAddressRepository(ILogger<NetworkAddress> logger, ApplicationDbContext dbContext) : INetworkAddressRepository
{
    //TODO handling of emergency DB and logging
    public async Task AddAsync(NetworkAddress record, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        dbContext.NetworkAddresses.Add(record);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(NetworkAddress record, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        dbContext.Remove(record);
        await dbContext.SaveChangesAsync();
    }

    public async Task<NetworkAddress?> FindByDomainUrlAsync(string domainUrl, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await dbContext
        .NetworkAddresses
        .FirstOrDefaultAsync(n => n.KnownDomains.Any(d => d.IsSameDomain(domainUrl)));
    }

    public async Task<NetworkAddress?> FindByIPAsync(string ip, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await dbContext.NetworkAddresses.FirstOrDefaultAsync(n => n.IP.Equals(ip, StringComparison.InvariantCultureIgnoreCase));
    }

    public async Task<IReadOnlyCollection<NetworkAddress>> GetAllAsync(Func<NetworkAddress,bool> filter, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await dbContext.NetworkAddresses
            .Where(n => filter(n))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task UpdateAsync(NetworkAddress record, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        dbContext.Update(record);
        await dbContext.SaveChangesAsync();
    }
}
