namespace GeoSnap.Application.Interfaces;
public interface IDnsResolvingService
{
    Task<string[]> GetIpsForDomainAsync(string networkAddress, CancellationToken cancellationToken);
}