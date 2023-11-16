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
    public const string Ip = @"^\s*(\d{1,3}\.){3}\d{1,3}\s*$";
}
