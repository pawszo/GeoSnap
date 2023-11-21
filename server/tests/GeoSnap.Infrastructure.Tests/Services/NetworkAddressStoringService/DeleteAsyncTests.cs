using Moq;
using GeoSnap.Domain.Enums;
using GeoSnap.Domain.Entities;
using GeoSnap.Infrastructure.Tests.TestData;

namespace GeoSnap.Infrastructure.Tests.Services.NetworkAddressStoringService;
[TestFixture]
public class DeleteAsyncTests : NetworkAddressStoringServiceTestsBase
{
    [Test]
    public async Task DeleteAsync_ReturnsTrue_WhenRecordWithGivenIpWasFoundAndDeleted()
    {
        // Arrange
        string ip = "192.168.1.1";
        RepositoryMock.Setup(r => r.FindByIPAsync(ip, It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestDataBuilder.CreateNetworkAddress(ip, "mydomain.com", ProtocolVersion.IPv4));
        RepositoryMock.Setup(r => r.Delete(It.IsAny<NetworkAddress>()))
            .Returns(true);
        Service = new GeoSnap.Infrastructure.Services.NetworkAddressStoringService(RepositoryMock.Object, LoggerMock.Object, CacheMock.Object);

        // Act
        var result = await Service.DeleteAsync(ip, CancellationToken.None);

        // Assert
        Assert.IsTrue(result);    
    }

    [Test]
    public async Task DeleteAsync_ReturnsFalse_WhenRecordWithGivenIpWasFoundButNotDeleted()
    {
        // Arrange
        string ip = "192.168.1.1";
        RepositoryMock.Setup(r => r.FindByIPAsync(ip, It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestDataBuilder.CreateNetworkAddress(ip, "mydomain.com", ProtocolVersion.IPv4));
        RepositoryMock.Setup(r => r.Delete(It.IsAny<NetworkAddress>()))
            .Returns(false);
        Service = new GeoSnap.Infrastructure.Services.NetworkAddressStoringService(RepositoryMock.Object, LoggerMock.Object, CacheMock.Object);

        // Act
        var result = await Service.DeleteAsync(ip, CancellationToken.None);

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public async Task DeleteAsync_ReturnsFalse_WhenRecordWithGivenIpWasNotFound()
    {
        // Arrange
        string ip = "192.168.1.1";
        RepositoryMock.Setup(r => r.FindByIPAsync(ip, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as NetworkAddress);
        RepositoryMock.Setup(r => r.Delete(It.IsAny<NetworkAddress>()))
            .Returns(false);
        Service = new GeoSnap.Infrastructure.Services.NetworkAddressStoringService(RepositoryMock.Object, LoggerMock.Object, CacheMock.Object);

        // Act
        var result = await Service.DeleteAsync(ip, CancellationToken.None);

        // Assert
        Assert.IsFalse(result);
    }
}
