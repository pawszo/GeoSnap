using MediatR;
using GeoSnap.Application.Interfaces;

namespace GeoSnap.Application.Commands;
public record class DeleteNetworkAddressDataCommand(string NetworkAddress) : IRequest<bool>;

public class DeleteNetworkAddressDataCommandHandler(INetworkAddressStoringService store) : IRequestHandler<DeleteNetworkAddressDataCommand, bool>
{
    public async Task<bool> Handle(DeleteNetworkAddressDataCommand request, CancellationToken cancellationToken)
    {
       return await store.DeleteAsync(request.NetworkAddress, cancellationToken);
    }
}
