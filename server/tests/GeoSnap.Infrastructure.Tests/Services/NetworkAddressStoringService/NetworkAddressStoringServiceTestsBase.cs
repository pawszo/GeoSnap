using Moq;
using GeoSnap.Domain.Entities;
using Microsoft.Extensions.Logging;
using GeoSnap.Application.Interfaces;

namespace GeoSnap.Infrastructure.Tests.Services.NetworkAddressStoringService;
[TestFixture]
public abstract class NetworkAddressStoringServiceTestsBase
{
    protected INetworkAddressStoringService Service;
    protected Mock<INetworkAddressRepository> NetworkAddressRepositoryMock;
    protected Mock<ILogger<NetworkAddress>> LoggerMock;

    [SetUp]
    public void SetUp()
    {
        NetworkAddressRepositoryMock = new Mock<INetworkAddressRepository>();
        LoggerMock = new Mock<ILogger<NetworkAddress>>();
        Service = new GeoSnap.Infrastructure.Services.NetworkAddressStoringService(NetworkAddressRepositoryMock.Object, LoggerMock.Object);
    }
}
