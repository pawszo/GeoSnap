using GeoSnap.Domain.Extensions;

namespace GeoSnap.Domain.Tests.Extensions;
[TestFixture]
public class DecimalExtensionsTests
{
    [TestCase(90d)]
    [TestCase(0)]
    [TestCase(-90d)]
    public void IsValidLatitude_ReturnsTrue_WhenGivenNumberIsInRange(decimal latitude)
    {
        // Arrange

        // Act
        var result = latitude.IsValidLatitude();

        // Assert
        Assert.IsTrue(result);
    }

    [TestCase(90.1d)]
    [TestCase(-90.1d)]
    public void IsValidLatitude_ReturnsFalse_WhenGivenNumberExceedsRange(decimal latitude)
    {
        // Arrange

        // Act
        var result = latitude.IsValidLatitude();

        // Assert
        Assert.IsFalse(result);
    }

    [TestCase(180d)]
    [TestCase(0)]
    [TestCase(-180d)]
    public void IsValidLongitude_ReturnsTrue_WhenGivenNumberIsInRange(decimal longitude)
    {
        // Arrange

        // Act
        var result = longitude.IsValidLongitude();

        // Assert
        Assert.IsTrue(result);
    }

    [TestCase(180.1d)]
    [TestCase(-180.1d)]
    public void IsValidlongitude_ReturnsFalse_WhenGivenNumberExceedsRange(decimal longitude)
    {
        // Arrange

        // Act
        var result = longitude.IsValidLongitude();

        // Assert
        Assert.IsFalse(result);
    }
}
