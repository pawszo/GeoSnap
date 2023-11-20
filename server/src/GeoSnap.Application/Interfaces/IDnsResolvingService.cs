namespace GeoSnap.Application.Interfaces;
public interface IDnsResolvingService
{
    Task<string?> GetIpForDomainAsync(string networkAddress, CancellationToken cancellationToken);
}