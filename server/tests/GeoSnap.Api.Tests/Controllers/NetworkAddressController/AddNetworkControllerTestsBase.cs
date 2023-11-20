using Moq;
using MediatR;

namespace GeoSnap.Api.Tests.Controllers.NetworkAddressController;
[TestFixture]
public abstract class AddNetworkControllerTestsBase
{
    protected Mock<ISender> SenderMock;
    protected GeoSnap.Api.Controllers.NetworkAddressController Controller;

    [SetUp]
    public void Setup()
    {
        SenderMock = new Mock<ISender>();
        Controller = new GeoSnap.Api.Controllers.NetworkAddressController();
    }
}
