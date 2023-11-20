namespace GeoSnap.Domain.Extensions;
public static class DecimalExtensions
{
    public static bool IsValidLatitude(this decimal latitude)
    {
        return latitude >= -90 && latitude <= 90;
    }

    public static bool IsValidLongitude(this decimal longitude)
    {
        return longitude >= -180 && longitude <= 180;
    }
}
