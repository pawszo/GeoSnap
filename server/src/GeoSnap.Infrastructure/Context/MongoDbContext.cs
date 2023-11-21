using MongoDB.Driver;
using GeoSnap.Domain.Entities;

namespace GeoSnap.Infrastructure.Context;
public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<NetworkAddress> NetworkAddresses => _database.GetCollection<NetworkAddress>("NetworkAddresses");
    public IMongoCollection<NetworkAddressGeoLocation> NetworkAddressGeoLocations => _database.GetCollection<NetworkAddressGeoLocation>("NetworkAddressGeoLocations");
}
