using MediatR;
using GeoSnap.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using GeoSnap.Domain.Extensions;
using GeoSnap.Application.Queries;

namespace GeoSnap.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class NetworkAddressController : ControllerBase
{
    [HttpGet("{networkAddress}", Name = "GetRecentLocation")]
    public async Task<ActionResult<NetworkAddressDto>> GetRecentLocationAsync(ISender sender, [FromRoute] string networkAddress)
    {
        if(networkAddress is null || !networkAddress.IsValidNetworkAddress())
        {
            return BadRequest("Provided network address is not a valid IP or URL");
        }

        var result = await sender.Send(new GetNetworkAddressRecentGeoLocationQuery(networkAddress));

        return result is null ? NotFound() : Ok(result);
    }

}
