using Moq;
using GeoSnap.Domain.Enums;
using GeoSnap.Domain.Entities;
using GeoSnap.Application.Interfaces;
using GeoSnap.Infrastructure.Tests.TestData;

namespace GeoSnap.Infrastructure.Tests.Services.NetworkAddressStoringService;
[TestFixture]
public class SaveAsyncTests : NetworkAddressStoringServiceTestsBase
{
    [Test]
    public async Task SaveAsync_CreatesNewRecord_WhenRecordWithGivenIpWasNotFound()
    {
        // Arrange
        string ip = "192.168.1.1";
        var dto = TestDataBuilder.CreateNetworkAddressDto(ip, "mydomain.com", ProtocolVersion.IPv4);
        var entity = dto.MapTo();
        RepositoryMock.Setup(r => r.FindByIPAsync(ip, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as NetworkAddress);
        RepositoryMock.Setup(r => r.AddAsync(It.IsAny<NetworkAddress>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);
        Service = new GeoSnap.Infrastructure.Services.NetworkAddressStoringService(RepositoryMock.Object, LoggerMock.Object, CacheMock.Object);

        // Act
        var result = await Service.SaveAsync(dto, CancellationToken.None);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(() => dto.IP == result.IP && dto.RecentGeoLocation.IP.Equals(result.RecentGeoLocation.IP));
        Assert.That(RepositoryMock.Invocations[1].Method.Name == nameof(INetworkAddressRepository.AddAsync));
    }

    [Test]
    public async Task SaveAsync_UpdatesExistingRecordAndReturnsLatestGeoLocation_WhenRecordWithGivenIpWasFound()
    {
        // Arrange
        string ip = "192.168.1.1";
        var dto = TestDataBuilder.CreateNetworkAddressDto(ip, "mydomain.com", ProtocolVersion.IPv4);
        var entity = dto.MapTo();
        RepositoryMock.Setup(r => r.FindByIPAsync(ip, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);
        RepositoryMock.Setup(r => r.AddAsync(It.IsAny<NetworkAddress>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);
        Service = new GeoSnap.Infrastructure.Services.NetworkAddressStoringService(RepositoryMock.Object, LoggerMock.Object, CacheMock.Object);

        // Act
        var result = await Service.SaveAsync(dto, CancellationToken.None);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(() => dto.IP == result.IP && dto.RecentGeoLocation.IP.Equals(result.RecentGeoLocation.IP));
        Assert.That(RepositoryMock.Invocations[1].Method.Name == nameof(INetworkAddressRepository.Update));
    }

    [Test]
    public async Task SaveAsync_ReturnsDtoFromInput_WhenRecordWithGivenIpWasNotFoundAndCouldNotBeCreated()
    {
        // Arrange
        string ip = "192.168.1.1";
        var dto = TestDataBuilder.CreateNetworkAddressDto(ip, "mydomain.com", ProtocolVersion.IPv4);
        var entity = dto.MapTo();
        RepositoryMock.Setup(r => r.FindByIPAsync(ip, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as NetworkAddress);
        RepositoryMock.Setup(r => r.AddAsync(It.IsAny<NetworkAddress>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as NetworkAddress);
        Service = new GeoSnap.Infrastructure.Services.NetworkAddressStoringService(RepositoryMock.Object, LoggerMock.Object, CacheMock.Object);

        // Act
        var result = await Service.SaveAsync(dto, CancellationToken.None);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(RepositoryMock.Invocations[1].Method.Name == nameof(INetworkAddressRepository.AddAsync));
        Assert.AreEqual(dto, result);
    }

    [Test]
    public async Task SaveAsync_ReturnsDtoFromInput_WhenRecordWithGivenIpWasFoundButCouldNotBeUpdated()
    {
        // Arrange
        string ip = "192.168.1.1";
        var dto = TestDataBuilder.CreateNetworkAddressDto(ip, "mydomain.com", ProtocolVersion.IPv4);
        var entity = dto.MapTo();
        RepositoryMock.Setup(r => r.FindByIPAsync(ip, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);
        RepositoryMock.Setup(r => r.Update(It.IsAny<NetworkAddress>()))
            .Returns(null as NetworkAddress);
        Service = new GeoSnap.Infrastructure.Services.NetworkAddressStoringService(RepositoryMock.Object, LoggerMock.Object, CacheMock.Object);

        // Act
        var result = await Service.SaveAsync(dto, CancellationToken.None);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(RepositoryMock.Invocations[1].Method.Name == nameof(INetworkAddressRepository.Update));
        Assert.AreEqual(dto, result);
    }
}
