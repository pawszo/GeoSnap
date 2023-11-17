using GeoSnap.Domain.Entities;
using GeoSnap.Domain.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using GeoSnap.Application.Interfaces;

namespace GeoSnap.Infrastructure.Repositories;
public class NetworkAddressRepository : INetworkAddressRepository
{
    private readonly ILogger<NetworkAddress> _logger;
    private readonly ApplicationDbContext _dbContext;
    //private readonly ApplicationDbContext _backupDbContext;

    public NetworkAddressRepository(ILogger<NetworkAddress> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task AddAsync(NetworkAddress record, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _dbContext.NetworkAddresses.Add(record);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(NetworkAddress record, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _dbContext.Remove(record);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<NetworkAddress?> FindByDomainUrlAsync(string domainUrl, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await _dbContext
        .NetworkAddresses
        .FirstOrDefaultAsync(n => n.KnownDomains.Any(d => d.IsSameDomain(domainUrl)));
    }

    public async Task<NetworkAddress?> FindByIPAsync(string ip, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await _dbContext.NetworkAddresses.FirstOrDefaultAsync(n => n.IP.Equals(ip, StringComparison.InvariantCultureIgnoreCase));
    }

    public async Task<IReadOnlyCollection<NetworkAddress>> GetAllAsync(Func<NetworkAddress,bool> filter, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await _dbContext.NetworkAddresses
            .Where(n => filter(n))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task UpdateAsync(NetworkAddress record, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _dbContext.Update(record);
        await _dbContext.SaveChangesAsync();
    }
}
