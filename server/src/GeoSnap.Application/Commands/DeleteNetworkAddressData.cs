using MediatR;
using GeoSnap.Domain.Exceptions;
using GeoSnap.Domain.Extensions;
using GeoSnap.Application.Interfaces;

namespace GeoSnap.Application.Commands;
public record class DeleteNetworkAddressDataCommand(string NetworkAddress) : IRequest<bool>;

public class DeleteNetworkAddressDataCommandHandler(
    INetworkAddressStoringService store,
    IDnsResolvingService dnsResolver) : IRequestHandler<DeleteNetworkAddressDataCommand, bool>
{
    public async Task<bool> Handle(DeleteNetworkAddressDataCommand request, CancellationToken cancellationToken)
    {

        if(request.NetworkAddress.TryGetValidDomainUrl(out string domainUrl))
        {
            var resolvedIps = await dnsResolver.GetIpsForDomainAsync(domainUrl, cancellationToken);
            if(resolvedIps == null || resolvedIps.Length == 0)
            {
                throw new DnsResolvingException(domainUrl);
            }
            bool hasDeletedAny = false;
            foreach(var ip in resolvedIps)
            {
                if(ip.TryGetValidIp(out string validIp, out _))
                {
                    var isDeleted = await store.DeleteAsync(ip, cancellationToken);
                    hasDeletedAny = isDeleted || hasDeletedAny;
                }
            }
            return hasDeletedAny;
        }

        request.NetworkAddress.TryGetValidIp(out string ipAddress, out _);
       
        return await store.DeleteAsync(ipAddress, cancellationToken);
    }
}
