using MediatR;
using GeoSnap.Domain.Entities;
using GeoSnap.Application.Dtos;
using GeoSnap.Domain.Extensions;
using GeoSnap.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using GeoSnap.Application.Interfaces;

namespace GeoSnap.Application.Commands;
public record class AddNetworkAddressDataCommand() : IRequest<NetworkAddressDto>
{
    public required string NetworkAddress { get; init; }
    public decimal Latitude { get; init; }
    public decimal Longitude { get; init; }
    public string? ContinentCode { get; init; }
    public string? CountryCode { get; init; }
    public string? RegionCode { get; init; }
    public string? City { get; init; }
    public string? ZipCode { get; init; }
}

public class AddNetworkAddressDataCommandHandler(
    ILogger<NetworkAddress> logger,
    INetworkAddressStoringService store,
    IDnsResolvingService dnsResolver) : IRequestHandler<AddNetworkAddressDataCommand, NetworkAddressDto>
{

    public async Task<NetworkAddressDto> Handle(AddNetworkAddressDataCommand request, CancellationToken cancellationToken)
    {
        if(request.NetworkAddress.TryGetValidIp(out string ip, out var protocolVersion))
        {
            return await store.SaveAsync(new NetworkAddressDto
            {
                IP = ip,
                Version = protocolVersion,
                RecentGeoLocation = new NetworkAddressGeoLocationDto
                {
                    IP = ip,
                    ProtocolVersion = protocolVersion,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    DataProviderName = "Manual",
                    CapturedAt = DateTime.UtcNow,
                    ContinentCode = request.ContinentCode ?? string.Empty,
                    CountryCode = request.CountryCode ?? string.Empty,
                    RegionCode = request.RegionCode ?? string.Empty,
                    City = request.City ?? string.Empty,
                    ZipCode = request.ZipCode ?? string.Empty
                }
            }, cancellationToken);
        }

        request.NetworkAddress.TryGetValidDomainUrl(out string domainUrl);
        
        var resolvedIp = await dnsResolver.GetIpForDomainAsync(domainUrl, cancellationToken);
        if(resolvedIp == null || !resolvedIp.TryGetValidIp(out string validIp, out protocolVersion))
        {
            logger.LogWarning("Failed to resolve IP for domain {DomainUrl}", domainUrl);
            throw new DnsResolvingException(domainUrl);
        }

        return await store.SaveAsync(new NetworkAddressDto
        {
            IP = validIp,
            Version = protocolVersion,
            Domain = domainUrl,
            RecentGeoLocation = new NetworkAddressGeoLocationDto
            {
                IP = validIp,
                ProtocolVersion = protocolVersion,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                DataProviderName = "Manual",
                CapturedAt = DateTime.UtcNow,
                ContinentCode = request.ContinentCode ?? string.Empty,
                CountryCode = request.CountryCode ?? string.Empty,
                RegionCode = request.RegionCode ?? string.Empty,
                City = request.City ?? string.Empty,
                ZipCode = request.ZipCode ?? string.Empty
            }
        }, cancellationToken);
    }
}
