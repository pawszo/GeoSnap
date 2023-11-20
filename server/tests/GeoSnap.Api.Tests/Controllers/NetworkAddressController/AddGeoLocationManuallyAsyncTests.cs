using Moq;
using Microsoft.AspNetCore.Mvc;
using GeoSnap.Application.Dtos;
using GeoSnap.Application.Commands;

namespace GeoSnap.Api.Tests.Controllers.NetworkAddressController;
[TestFixture]
public class AddGeoLocationManuallyAsyncTests : AddNetworkControllerTestsBase
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
    public async Task AddGeoLocationManuallyAsync_ReturnsBadRequest_WhenNetworkAddressIsNotIpOrValidUrl(string networkAddress)
    {
        // Arrange

        // Act
        var result = await Controller.AddGeoLocationManuallyAsync(SenderMock.Object,  new AddNetworkAddressDataCommand { NetworkAddress = networkAddress, Latitude = 0, Longitude = 0 });

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
    public async Task AddGeoLocationManuallyAsync_ReturnsBadRequest_WhenNetworkAddressIsValidIpOrUrl(string networkAddress)
    {
        // Arrange
        SenderMock.Setup(s => s.Send(It.IsAny<AddNetworkAddressDataCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<NetworkAddressDto>()
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
        var result = await Controller.AddGeoLocationManuallyAsync(SenderMock.Object, new AddNetworkAddressDataCommand { NetworkAddress = networkAddress, Latitude = 0, Longitude = 0 });

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotInstanceOf<BadRequestObjectResult>(result.Result);
    }

    [TestCase(-180.1d)]
    [TestCase(180.1d)]
    public async Task AddGeoLocationManuallyAsync_ReturnsBadRequest_WhenLongitudeExceedsValidRange(decimal longitude)
    {
        // Arrange
        SenderMock.Setup(s => s.Send(It.IsAny<AddNetworkAddressDataCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<NetworkAddressDto>()
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
        var result = await Controller.AddGeoLocationManuallyAsync(SenderMock.Object, new AddNetworkAddressDataCommand { NetworkAddress = "192.168.1.1", Latitude = 0, Longitude = longitude });

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
    }

    [TestCase(-90.1d)]
    [TestCase(90.1d)]
    public async Task AddGeoLocationManuallyAsync_ReturnsBadRequest_WhenLatitudeExceedsValidRange(decimal latitude)
    {
        // Arrange
        SenderMock.Setup(s => s.Send(It.IsAny<AddNetworkAddressDataCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<NetworkAddressDto>()
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
        var result = await Controller.AddGeoLocationManuallyAsync(SenderMock.Object, new AddNetworkAddressDataCommand { NetworkAddress = "192.168.1.1", Latitude = latitude, Longitude = 0 });

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
    }

    [Test]
    public async Task AddGeoLocationManuallyAsync_ReturnsOk_WhenInputIsValid()
    {
        // Arrange
        SenderMock.Setup(s => s.Send(It.IsAny<AddNetworkAddressDataCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<NetworkAddressDto>()
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
        var result = await Controller.AddGeoLocationManuallyAsync(SenderMock.Object, new AddNetworkAddressDataCommand { NetworkAddress = "192.168.1.1", Latitude = 0, Longitude = 0 });

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
    }


}
