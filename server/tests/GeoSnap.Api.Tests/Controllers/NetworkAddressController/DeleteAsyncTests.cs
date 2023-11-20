using Moq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using GeoSnap.Application.Commands;

namespace GeoSnap.Api.Tests.Controllers.NetworkAddressController;
[TestFixture]
public class DeleteAsyncTests : AddNetworkControllerTestsBase
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
    public async Task DeleteAsync_ReturnsBadRequest_WhenNetworkAddressIsNotIpOrValidUrl(string networkAddress)
    {
        // Arrange

        // Act
        var result = await Controller.DeleteAsync(SenderMock.Object, networkAddress);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
    }

    [TestCase("192.168.1.1")]
    [TestCase("google.com.pl")]
    [TestCase("http://www.wp.pl/#234")]
    [TestCase("https://test.net")]
    [TestCase("ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff")]
    [TestCase("www.onet.pl/artykul/costam?kjsdfksdnf=jsdbfjdsb&inne=1")]
    [TestCase("  yahoo.com  ")]
    public async Task DeleteAsync_NotReturnsBadRequest_WhenNetworkAddressIsValidIpOrUrl(string networkAddress)
    {
        // Arrange
        SenderMock.Setup(s => s.Send(It.IsAny<DeleteNetworkAddressDataCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        var result = await Controller.DeleteAsync(SenderMock.Object, networkAddress);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotInstanceOf<BadRequestObjectResult>(result);
    }

    [Test]
    public async Task DeleteAsync_ReturnsNotFound_WhenQueryReturnsFalse()
    {
        // Arrange
        SenderMock.Setup(s => s.Send(It.IsAny<DeleteNetworkAddressDataCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        var result = await Controller.DeleteAsync(SenderMock.Object, "test.pl");

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<NotFoundResult>(result);
    }


    [Test]
    public async Task DeleteAsync_ReturnsOk_WhenQueryReturnsTrue()
    {
        // Arrange
        SenderMock.Setup(s => s.Send(It.IsAny<DeleteNetworkAddressDataCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await Controller.DeleteAsync(SenderMock.Object, "192.168.1.1");

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkResult>(result);
    }



}
