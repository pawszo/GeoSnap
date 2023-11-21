namespace GeoSnap.Domain.Constants;
public static class RegexPatterns
{
    /// <summary>
    /// Group 2 contains the domain name
    /// </summary>
    public const string DomainUrl = @"^\s*(https?://)?((?:[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?\.)+[a-zA-Z]{2,}).*$";

    /// <summary>
    /// Group 1 contains the IP address
    /// </summary>
    public const string IpV4 = @"^\s*((\d{1,3}\.){3}\d{1,3})\s*$";

    /// <summary>
    /// Group 1 contains the IP address
    /// </summary>
    public const string IpV6 = @"^\s*((?:[0-9a-fA-F]{0,4}:){3,7}[0-9a-fA-F]{1,4})\s*$";
}
