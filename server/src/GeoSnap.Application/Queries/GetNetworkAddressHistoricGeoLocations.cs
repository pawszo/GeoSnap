﻿using MediatR;
using FluentValidation;
using GeoSnap.Domain.Entities;
using GeoSnap.Application.Dtos;
using GeoSnap.Domain.Exceptions;
using GeoSnap.Domain.Extensions;
using Microsoft.Extensions.Logging;
using GeoSnap.Application.Interfaces;

namespace GeoSnap.Application.Queries;
public record class GetNetworkAddressHistoricGeoLocationsQuery(string NetworkAddress) : IRequest<IList<NetworkAddressHistoryDto>?>;

public class GetNetworkAddressHistoricGeoLocationsQueryHandler(
    ILogger<NetworkAddress> logger, 
    INetworkAddressStoringService store,
    IDnsResolvingService dnsResolver) : IRequestHandler<GetNetworkAddressHistoricGeoLocationsQuery, IList<NetworkAddressHistoryDto>?>
{
    public async Task<IList<NetworkAddressHistoryDto>?> Handle(GetNetworkAddressHistoricGeoLocationsQuery request, CancellationToken cancellationToken)
    {
        if(request.NetworkAddress.TryGetValidDomainUrl(out string domainUrl))
        {
            var resolvedIps = await dnsResolver.GetIpsForDomainAsync(domainUrl, cancellationToken);
            if(resolvedIps == null || resolvedIps.Length == 0)
            {
                throw new DnsResolvingException(domainUrl);
            }
            IList<NetworkAddressHistoryDto> results = new List<NetworkAddressHistoryDto>();
            foreach(var ip in resolvedIps)
            {
                if(ip.TryGetValidIp(out string validIp, out _))
                {
                    var record = await store.GetHistoryAsync(ip, cancellationToken);
                    if(record is not null) results.Add(record);
                }
            }
            return results.Count > 0 ? results : null;
        }

        request.NetworkAddress.TryGetValidIp(out string ipAddress, out _);
        var result = await store.GetHistoryAsync(ipAddress, cancellationToken);

        if(result is null)
        {
            logger.LogWarning("No records were found for {NetworkAddress}", request.NetworkAddress);
            return null;
        }

        return [result];
    }
}

public class GetNetworkAddressHistoricGeoLocationsQueryValidator : AbstractValidator<GetNetworkAddressHistoricGeoLocationsQuery>
{
    public GetNetworkAddressHistoricGeoLocationsQueryValidator()
    {
        RuleFor(q => q.NetworkAddress)
            .NotEmpty().WithMessage("Network address is required");

        RuleFor(q => q.NetworkAddress)
            .Must(n => n.IsValidNetworkAddress())
            .WithMessage("Network address must be a valid IP or domain URL.");
    }
}