using Moq;
using GeoSnap.Domain.Enums;
using GeoSnap.Domain.Entities;
using GeoSnap.Application.Dtos;
using GeoSnap.Infrastructure.Tests.TestData;

namespace GeoSnap.Infrastructure.Tests.Services.NetworkAddressStoringService;
[TestFixture]
public class GetHistoryAsyncTests : NetworkAddressStoringServiceTestsBase
{
    [Test]
    public async Task GetHistoryAsync_ReturnsDto_WhenRecordWithGivenIpWasFound()
    {
        // Arrange
        string ip = "192.168.1.1";
        var entity = TestDataBuilder.CreateNetworkAddress(ip, "mydomain.com", ProtocolVersion.IPv4);
        var expectedDto = NetworkAddressHistoryDto.MapFrom(entity);
        RepositoryMock.Setup(r => r.FindByIPAsync(ip, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);
        Service = new GeoSnap.Infrastructure.Services.NetworkAddressStoringService(RepositoryMock.Object, LoggerMock.Object, CacheMock.Object);

        // Act
        var result = await Service.GetHistoryAsync(ip, CancellationToken.None);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(() => expectedDto.IP == result.IP && expectedDto.GeoLocations.First().IP.Equals(result.GeoLocations.First().IP));
    }

    [Test]
    public async Task GetHistoryAsync_ReturnsNull_WhenRecordWithGivenIpWasNotFound()
    {
        // Arrange
        string ip = "192.168.1.1";
        var entity = TestDataBuilder.CreateNetworkAddress(ip, "mydomain.com", ProtocolVersion.IPv4);
        RepositoryMock.Setup(r => r.FindByIPAsync(ip, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as NetworkAddress);
        Service = new GeoSnap.Infrastructure.Services.NetworkAddressStoringService(RepositoryMock.Object, LoggerMock.Object, CacheMock.Object);

        // Act
        var result = await Service.GetHistoryAsync(ip, CancellationToken.None);

        // Assert
        Assert.IsNull(result);
    }
}
