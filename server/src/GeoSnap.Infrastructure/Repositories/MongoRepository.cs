using MongoDB.Driver;
using GeoSnap.Domain.Entities;
using Microsoft.Extensions.Logging;
using GeoSnap.Application.Interfaces;
using GeoSnap.Infrastructure.Context;

namespace GeoSnap.Infrastructure.Repositories;
internal class MongoRepository : INetworkAddressRepository
{
    private readonly MongoDbContext _dbContext;
    private readonly ILogger<NetworkAddress> _logger;

    public MongoRepository(MongoDbContext dbContext, ILogger<NetworkAddress> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<NetworkAddress> AddAsync(NetworkAddress record, CancellationToken cancellationToken)
    {
        await _dbContext.NetworkAddresses.InsertOneAsync(record, cancellationToken: cancellationToken);
        return record;
    }

    public bool Delete(NetworkAddress record)
    {
        var result = _dbContext.NetworkAddresses.DeleteOne(record.IP);
        return result.IsAcknowledged;
    }

    public async Task<NetworkAddress?> FindByIPAsync(string ip, CancellationToken cancellationToken)
    {
        var filter = Builders<NetworkAddress>.Filter.Eq(n => n.IP, ip);
        var entities = await _dbContext.NetworkAddresses.FindAsync(filter, cancellationToken: cancellationToken);
        return entities.FirstOrDefault();
    }

    public async Task<IReadOnlyCollection<NetworkAddress>> GetAllAsync(Func<NetworkAddress, bool> filter, CancellationToken cancellationToken)
    {
        var dbFilter = Builders<NetworkAddress>.Filter.Where(n => filter(n));
        var results = await _dbContext.NetworkAddresses.FindAsync(dbFilter, cancellationToken: cancellationToken);
        return results.ToList();
    }

    public NetworkAddress Update(NetworkAddress record)
    {
        var dbFilter = Builders<NetworkAddress>.Filter.Where(n => record.IP == n.IP);
        var dbUpdate = Builders<NetworkAddress>.Update.Combine();

        var result = _dbContext.NetworkAddresses.UpdateOne(dbFilter, dbUpdate);
        return result.IsAcknowledged ? record : null;
    }
}
