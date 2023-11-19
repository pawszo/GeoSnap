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
        await dbContext.NetworkAddresses.AddAsync(record, cancellationToken);
        await dbContext.SaveChangesAsync();
    }

    public void Delete(NetworkAddress record)
    {
        dbContext.Remove(record);
        dbContext.SaveChanges();
    }

    public async Task<NetworkAddress?> FindByDomainUrlAsync(string domainUrl, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await dbContext
        .NetworkAddresses
        .AsNoTracking()
        .Include(n => n.GeoLocations)
        .FirstOrDefaultAsync(n => n.Domain == domainUrl, cancellationToken);
    }

    public async Task<NetworkAddress?> FindByIPAsync(string ip, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var networkAddress = await dbContext.FindAsync<NetworkAddress>(ip, cancellationToken);
        if(networkAddress is not null)
        {
            await dbContext.Entry(networkAddress).Collection(n => n.GeoLocations).LoadAsync(cancellationToken);
        }
        return networkAddress;
    }

    public async Task<IReadOnlyCollection<NetworkAddress>> GetAllAsync(Func<NetworkAddress,bool> filter, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await dbContext.NetworkAddresses
            .Where(n => filter(n))
            .Include(n => n.GeoLocations)
            .AsNoTracking()
            .ToListAsync();
    }

    public void Update(NetworkAddress record)
    {
        dbContext.Update(record);
        dbContext.SaveChanges();
    }
}
