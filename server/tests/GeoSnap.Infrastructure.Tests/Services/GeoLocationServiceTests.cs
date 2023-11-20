using Moq;
using GeoSnap.Application.Dtos;
using GeoSnap.Application.Interfaces;
using GeoSnap.Infrastructure.Services;
using GeoSnap.Infrastructure.Tests.TestData;

namespace GeoSnap.Infrastructure.Tests.Services;
[TestFixture]
public class GeoLocationServiceTests
{
    private Mock<IGeoLocationDataProvider> _mainProviderMock;
    private Mock<IGeoLocationDataProvider> _alternativeProviderMock;
    private IGeoLocationService _service;
    private NetworkAddressGeoLocationDto _ipStackNormalizedDto;
    private NetworkAddressGeoLocationDto _ipifyNormalizedDto;
    private string _testIpAddress = "192.168.1.1";

    [SetUp]
    public void SetUp()
    {
        _mainProviderMock = new Mock<IGeoLocationDataProvider>();
        _alternativeProviderMock = new Mock<IGeoLocationDataProvider>();
        _ipStackNormalizedDto = TestDataBuilder.CreateGeoLocationDto(_testIpAddress, DateTime.UtcNow, dataProviderName: "IpStack");
        _ipifyNormalizedDto = TestDataBuilder.CreateGeoLocationDto(_testIpAddress, DateTime.UtcNow, dataProviderName: "Ipify");
        _mainProviderMock.Setup(p => p.FindIPAsync(_testIpAddress, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_ipStackNormalizedDto);
        _alternativeProviderMock.Setup(p => p.FindIPAsync(_testIpAddress, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_ipifyNormalizedDto);
        _service = new GeoLocationService(_mainProviderMock.Object, _alternativeProviderMock.Object);
    }

    [Test]
    public async Task GetGeoLocationAsync_ReturnsDataFromMainProvider_WhenMainProviderReturnsData()
    {
        // Arrange

        // Act
        var result = await _service.GetGeoLocationAsync(_testIpAddress, CancellationToken.None);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(_ipStackNormalizedDto, result);
        Assert.That(_mainProviderMock.Invocations[0].Method.Name == nameof(IGeoLocationDataProvider.FindIPAsync));
        Assert.That(_alternativeProviderMock.Invocations.Count == 0);
    }

    [Test]
    public async Task GetGeoLocationAsync_ReturnsDataFromAlternativeProvider_WhenMainProviderNotReturnsData()
    {
        // Arrange
        _mainProviderMock.Setup(p => p.FindIPAsync(_testIpAddress, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as NetworkAddressGeoLocationDto);
        _service = new GeoLocationService(_mainProviderMock.Object, _alternativeProviderMock.Object);

        // Act
        var result = await _service.GetGeoLocationAsync(_testIpAddress, CancellationToken.None);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(_ipifyNormalizedDto, result);
        Assert.That(_mainProviderMock.Invocations[0].Method.Name == nameof(IGeoLocationDataProvider.FindIPAsync));
        Assert.That(_alternativeProviderMock.Invocations[0].Method.Name == nameof(IGeoLocationDataProvider.FindIPAsync));
    }

    [Test]
    public async Task GetGeoLocationAsync_ReturnsNull_WhenAllProvidersnotReturnData()
    {
        // Arrange
        _mainProviderMock.Setup(p => p.FindIPAsync(_testIpAddress, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as NetworkAddressGeoLocationDto);
        _alternativeProviderMock.Setup(p => p.FindIPAsync(_testIpAddress, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as NetworkAddressGeoLocationDto);
        _service = new GeoLocationService(_mainProviderMock.Object, _alternativeProviderMock.Object);

        // Act
        var result = await _service.GetGeoLocationAsync(_testIpAddress, CancellationToken.None);

        // Assert
        Assert.IsNull(result);
        Assert.That(_mainProviderMock.Invocations[0].Method.Name == nameof(IGeoLocationDataProvider.FindIPAsync));
        Assert.That(_alternativeProviderMock.Invocations[0].Method.Name == nameof(IGeoLocationDataProvider.FindIPAsync));
    }
}
