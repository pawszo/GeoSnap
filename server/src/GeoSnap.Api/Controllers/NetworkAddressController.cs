using MediatR;
using GeoSnap.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using GeoSnap.Application.Queries;
using GeoSnap.Application.Commands;

namespace GeoSnap.Api.Controllers;

/// <summary>
/// Geo location data operations
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class NetworkAddressController : ControllerBase
{
    /// <summary>
    /// Check latest geo location data for a network address. Record is persisted for history tracking.
    /// </summary>
    /// <param name="networkAddress">IP or URL</param>
    /// <returns>Most recent geo location that was available</returns>
    [HttpGet("geolocation/{networkAddress}")]
    public async Task<ActionResult<NetworkAddressDto[]>> GetRecentLocationAsync(ISender sender, [FromRoute] string networkAddress)
    {
        var result = await sender.Send(new GetNetworkAddressRecentGeoLocationQuery(networkAddress));

        return result is null || result.Count == 0 ? NotFound() : Ok(result);
    }

    /// <summary>
    /// Look up storage for historical data.
    /// </summary>
    /// <param name="networkAddress">IP or URL</param>
    /// <returns>geo locations history for a network address</returns>
    [HttpGet("history/{networkAddress}")]
    public async Task<ActionResult<NetworkAddressHistoryDto[]>> GetHistoryAsync(ISender sender, [FromRoute] string networkAddress)
    {
        var result = await sender.Send(new GetNetworkAddressHistoricGeoLocationsQuery(networkAddress));

        return result is null || result.Count == 0 ? NotFound() : Ok(result);
    }

    /// <summary>
    /// Wipe out all data related to a network address if exists.
    /// </summary>
    /// <param name="networkAddress">IP or URL</param>
    [HttpDelete("delete/{networkAddress}")]
    public async Task<ActionResult> DeleteAsync(ISender sender, [FromRoute] string networkAddress)
    {
        bool isRemoved = await sender.Send(new DeleteNetworkAddressDataCommand(networkAddress));

        return isRemoved ? Ok() : NotFound();
    }

    /// <summary>
    /// Add geo location data for a network address from external source manually.
    /// </summary>
    /// <param name="addNetworkAddressData">IP or URL and with related geo location data</param>
    /// <returns>Created record</returns>
    [HttpPost("geolocation")]
    public async Task<ActionResult<NetworkAddressDto[]>> AddGeoLocationManuallyAsync(ISender sender, [FromBody] AddNetworkAddressDataCommand addNetworkAddressData)
    {
        var result = await sender.Send(addNetworkAddressData);

        return Ok(result);
    }
}
