using GeoSnap.Application.Dtos;

namespace GeoSnap.Application.Interfaces;
public interface INetworkAddressStoringService
{
    /// <summary>
    /// Creates or updates a record in the store.
    /// </summary>
    /// <param name="recent">Data from external provider</param>
    /// <returns>Saved record updated with additional data if formerly existed such as known domains</returns>
    Task<NetworkAddressDto> SaveAsync(NetworkAddressDto recent, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves full geo location history for a network address
    /// </summary>
    /// <param name="ip"></param>
    /// <returns>null if no record exists</returns>
    Task<NetworkAddressHistoryDto?> GetHistoryAsync(string ip, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes the full history of a network address
    /// </summary>
    /// <param name="ip"></param>
    /// <returns></returns>
    Task<bool> DeleteAsync(string ip, CancellationToken cancellationToken);
}
