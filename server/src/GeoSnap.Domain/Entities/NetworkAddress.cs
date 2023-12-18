using MongoDB.Bson;
using GeoSnap.Domain.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace GeoSnap.Domain.Entities;
public class NetworkAddress : DomainObject
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string IP { get; set; }
    public required ProtocolVersion Version { get; set; }
    public string? Domain { get; set; }
    public ICollection<NetworkAddressGeoLocation> GeoLocations { get; set; }
}
