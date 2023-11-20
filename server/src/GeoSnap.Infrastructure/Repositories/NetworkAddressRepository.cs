using GeoSnap.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using GeoSnap.Application.Interfaces;

namespace GeoSnap.Infrastructure.Repositories;
public class NetworkAddressRepository(ILogger<NetworkAddress> logger, ApplicationDbContext dbContext) : INetworkAddressRepository
{
    //TODO handling of emergency DB and logging
    public async Task<NetworkAddress> AddAsync(NetworkAddress record, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var createdRecord = await dbContext.NetworkAddresses.AddAsync(record, cancellationToken);
        var changes = await dbContext.SaveChangesAsync();
        if(changes > 0) logger.LogInformation("Created geo location for {IP}", createdRecord.Entity.IP);

        return createdRecord.Entity;
    }

    public bool Delete(NetworkAddress record)
    {
        dbContext.Remove(record);
        return dbContext.SaveChanges() > 0;
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

    public NetworkAddress Update(NetworkAddress record)
    {
        var updatedRecord = dbContext.Update(record);
        var changes = dbContext.SaveChanges();
        if(changes > 0) logger.LogInformation("Updated geo location for {IP}", updatedRecord.Entity.IP);

        return updatedRecord.Entity;
    }
}
