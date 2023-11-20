﻿using Moq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using GeoSnap.Application.Dtos;
using GeoSnap.Application.Queries;

namespace GeoSnap.Api.Tests.Controllers.NetworkAddressController;
[TestFixture]
public class GetRecentLocationAsyncTests : AddNetworkControllerTestsBase
{

    [TestCase("")]
    [TestCase("invalidUrl")]
    [TestCase("test@mail.com")]
    [TestCase("http://not a valid url either")]
    [TestCase("192,168,1,1")]
    [TestCase("0:0:0")]
    [TestCase("cc.a0.12.10")]
    [TestCase("ffff:ffff:ffff:ffff:ffff:ffff:ffff:fffg")]
    [TestCase("mur-chiński.pl")]
    public async Task GetRecentLocationAsync_ReturnsBadRequest_WhenNetworkAddressIsNotIpOrValidUrl(string networkAddress)
    {
        // Arrange

        // Act
        var result = await Controller.GetRecentLocationAsync(SenderMock.Object, networkAddress);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
    }

    [TestCase("192.168.1.1")]
    [TestCase("google.com.pl")]
    [TestCase("http://www.wp.pl/#234")]
    [TestCase("https://test.net")]
    [TestCase("ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff")]
    [TestCase("www.onet.pl/artykul/costam?kjsdfksdnf=jsdbfjdsb&inne=1")]
    [TestCase("  yahoo.com  ")]
    public async Task GetRecentLocationAsync_NotReturnsBadRequest_WhenNetworkAddressIsValidIpOrUrl(string networkAddress)
    {
        // Arrange
        SenderMock.Setup(s => s.Send(It.IsAny<GetNetworkAddressRecentGeoLocationQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<NetworkAddressDto>());

        // Act
        var result = await Controller.GetRecentLocationAsync(SenderMock.Object, networkAddress);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotInstanceOf<BadRequestObjectResult>(result.Result);
    }

    [Test]
    public async Task GetRecentLocationAsync_ReturnsNotFound_WhenQueryReturnsNull()
    {
        // Arrange
        SenderMock.Setup(s => s.Send(It.IsAny<GetNetworkAddressRecentGeoLocationQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(null as IList<NetworkAddressDto>);

        // Act
        var result = await Controller.GetRecentLocationAsync(SenderMock.Object, "test.pl");

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<NotFoundResult>(result.Result);
    }

    [Test]
    public async Task GetRecentLocationAsync_ReturnsNotFound_WhenQueryReturnsEmptyList()
    {
        // Arrange
        SenderMock.Setup(s => s.Send(It.IsAny<GetNetworkAddressRecentGeoLocationQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<NetworkAddressDto>());

        // Act
        var result = await Controller.GetRecentLocationAsync(SenderMock.Object, "test.pl");

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<NotFoundResult>(result.Result);
    }

    [Test]
    public async Task GetRecentLocationAsync_ReturnsOk_WhenQueryReturnsResults()
    {
        // Arrange
        SenderMock.Setup(s => s.Send(It.IsAny<GetNetworkAddressRecentGeoLocationQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<NetworkAddressDto>()
            {
                new NetworkAddressDto {
                    IP = "192.168.1.1",
                    Version = Domain.Enums.ProtocolVersion.IPv4,
                    RecentGeoLocation = new NetworkAddressGeoLocationDto
                    {
                        City = "",
                        ContinentCode = "",
                        CountryCode = "",
                        DataProviderName = "",
                        IP = "192.168.1.1",
                        RegionCode = "",
                        ZipCode = "",
                        ProtocolVersion = Domain.Enums.ProtocolVersion.IPv4
                    }
                }
             });

        // Act
        var result = await Controller.GetRecentLocationAsync(SenderMock.Object, "192.168.1.1");

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
    }



}
