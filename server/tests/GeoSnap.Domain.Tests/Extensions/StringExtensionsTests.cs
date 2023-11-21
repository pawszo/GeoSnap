using System;
using System.Linq;
using System.Text;
using GeoSnap.Domain.Enums;
using System.Threading.Tasks;
using GeoSnap.Domain.Extensions;
using System.Collections.Generic;

namespace GeoSnap.Domain.Tests.Extensions;
[TestFixture]
public class StringExtensionsTests
{
    [TestCase("")]
    [TestCase("       ")]
    [TestCase("invalidUrl")]
    [TestCase("test@mail.com")]
    [TestCase("http://not a valid url either")]
    [TestCase("192,168,1,1")]
    [TestCase("0:0:0")]
    [TestCase("cc.a0.12.10")]
    [TestCase("ffff:ffff:ffff:ffff:ffff:ffff:ffff:fffg")]
    [TestCase("mur-chiński.pl")]
    public void IsValidNetworkAddress_ReturnsFalse_WhenNetworkAddressIsNotIpOrUrl(string networkAddress)
    {
        // Arrange

        // Act
        var result = networkAddress.IsValidNetworkAddress();

        // Assert
        Assert.IsFalse(result);
    }

    [TestCase("192.168.1.1")]
    [TestCase("google.com.pl")]
    [TestCase("http://www.wp.pl/#234")]
    [TestCase("https://test.net")]
    [TestCase("ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff")]
    [TestCase("www.onet.pl/artykul/costam?kjsdfksdnf=jsdbfjdsb&inne=1")]
    [TestCase("  yahoo.com  ")]
    public void IsValidNetworkAddress_ReturnsTrue_WhenNetworkAddressIsValidIpOrUrl(string networkAddress)
    {
        // Arrange

        // Act
        var result = networkAddress.IsValidNetworkAddress();

        // Assert
        Assert.IsTrue(result);
    }

    [TestCase("google.com.pl", "google.com.pl")]
    [TestCase("http://www.wp.pl/#234", "wp.pl")]
    [TestCase("https://test.net", "test.net")]
    [TestCase("www.test.pl/artykul/costam?kjsdfksdnf=jsdbfjdsb&inne=1", "test.pl")]
    [TestCase("  yahoo.com  ", "yahoo.com")]
    public void TryGetValidDomainUrl_ReturnsExtractedDomainName_WhenNetworkAddressIsValidUrl(string networkAddress, string expectedDomain)
    {
        // Arrange

        // Act
        var result = networkAddress.TryGetValidDomainUrl(out var domainUrl);

        // Assert
        Assert.IsTrue(result);
        Assert.IsNotNull(domainUrl);
        Assert.AreEqual(expectedDomain, domainUrl);
    }

    [TestCase("192.168.1.1", ProtocolVersion.IPv4)]
    [TestCase("0.0.0.0", ProtocolVersion.IPv4)]
    [TestCase("  2602:ffea:a::dead:beef ", ProtocolVersion.IPv6)]
    [TestCase(" ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff", ProtocolVersion.IPv6)]
    public void TryGetValidIp_ReturnsExtractedIpAndVersion_WhenNetworkAddressIsValidIP(string ip, ProtocolVersion expectedVersion)
    {
        // Arrange

        // Act
        var result = ip.TryGetValidIp(out var validIp, out var validVersion);

        // Assert
        Assert.IsTrue(result);
        Assert.IsNotNull(validIp);
        Assert.AreEqual(expectedVersion, validVersion);
    }
}
