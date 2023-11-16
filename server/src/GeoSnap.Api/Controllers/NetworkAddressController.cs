using MediatR;
using GeoSnap.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using GeoSnap.Domain.Extensions;
using GeoSnap.Application.Queries;
using GeoSnap.Application.Interfaces;

namespace GeoSnap.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class NetworkAddressController : ControllerBase
{
    //private readonly ILogger<WeatherForecastController> _logger;

    //private readonly IGeoLocationDataProvider _geoLocationDataProvider;

    [HttpGet("{networkAddress}", Name = "GetRecentLocation")]
    public async Task<ActionResult<NetworkAddressDto>> GetRecentLocationAsync(ISender sender, [FromRoute] string networkAddress)
    {
        if(networkAddress is null || !networkAddress.IsValidNetworkAddress())
        {
            return BadRequest("Provided network address is not a valid IP or URL");
        }

        var result = await sender.Send(new GetNetworkAddressRecentGeoLocationQuery(networkAddress));

        if(result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

}
