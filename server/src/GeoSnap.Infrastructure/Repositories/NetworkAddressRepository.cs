using GeoSnap.Domain.Entities;
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

    public async Task AddAsync(NetworkAddress record)
    {
        _dbContext.NetworkAddresses.Add(record);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(NetworkAddress record)
    {
        _dbContext.Remove(record);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<NetworkAddress?> FindByDomainUrlAsync(string domainUrl)
    {
        return await _dbContext.NetworkAddresses.FirstOrDefaultAsync(n => n.KnownDomains.Any(d => d == domainUrl));
    }

    public async Task<NetworkAddress?> FindByIPAsync(string ip)
    {
        return await _dbContext.NetworkAddresses.FirstOrDefaultAsync(n => n.IP == ip);
    }

    public async Task<IReadOnlyCollection<NetworkAddress>> GetAllAsync(Func<NetworkAddress,bool> filter)
    {
        return await _dbContext.NetworkAddresses
            .Where(n => filter(n))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task UpdateAsync(NetworkAddress record)
    {
        _dbContext.Update(record);
        await _dbContext.SaveChangesAsync();
    }
}
