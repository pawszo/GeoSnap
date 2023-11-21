using GeoSnap.Domain.Enums;
using GeoSnap.Domain.Constants;
using System.Text.RegularExpressions;

namespace GeoSnap.Domain.Extensions;
public static partial class StringExtensions
{
    [GeneratedRegex(RegexPatterns.IpV4, RegexOptions.IgnoreCase)]
    private static partial Regex IpV4Regex();

    [GeneratedRegex(RegexPatterns.IpV6, RegexOptions.IgnoreCase)]
    private static partial Regex IpV6Regex();

    [GeneratedRegex(RegexPatterns.DomainUrl, RegexOptions.IgnoreCase)]
    private static partial Regex DomainNameRegex();

    public static bool IsValidNetworkAddress(this string address) => address.TryGetValidDomainUrl(out _) || address.TryGetValidIp(out _, out _);

    public static bool TryGetValidIp(this string url, out string ip, out ProtocolVersion version)
    {
        var matchV4 = IpV4Regex().Match(url);
        if (matchV4.Success)
        {
            ip = matchV4.Groups[1].Value.ToLower();
            version = ProtocolVersion.IPv4;
            return true;
        }

        var matchV6 = IpV6Regex().Match(url);
        if (matchV6.Success)
        {
            ip = matchV6.Groups[1].Value.ToLower();
            version = ProtocolVersion.IPv6;
            return true;
        }

        ip = string.Empty;
        version = ProtocolVersion.Unknown;
        return false;
    }

    public static bool TryGetValidDomainUrl(this string url, out string domainUrl)
    {
        var match = DomainNameRegex().Match(url);
        if (match.Success)
        {
            domainUrl = match.Groups[2].Value.ToLower();
            if(domainUrl.StartsWith("www.")) domainUrl = domainUrl[4..];
            return true;
        }

        domainUrl = string.Empty;
        return false;
    }

    /// <summary>
    /// Compares two domain urls which were formerly returned by TryGetValidDomainUrl.
    /// Therefore assumes that url is trimmed, lowercase, with no protocol and no routes.
    /// </summary>
    /// <param name="domainUrl"></param>
    /// <param name="otherDomainUrl"></param>
    /// <returns></returns>
    public static bool IsSameDomain(this string domainUrl, string otherDomainUrl)
    {
        if (string.IsNullOrWhiteSpace(domainUrl) || string.IsNullOrWhiteSpace(otherDomainUrl)) return false;

        if (domainUrl.Equals(otherDomainUrl, StringComparison.InvariantCultureIgnoreCase)) return true;

        if (domainUrl.StartsWith("www.", StringComparison.InvariantCultureIgnoreCase))
        {
            domainUrl = domainUrl[4..];
        }

        if (otherDomainUrl.StartsWith("www.", StringComparison.InvariantCultureIgnoreCase))
        {
            otherDomainUrl = otherDomainUrl[4..];
        }

        return domainUrl.Equals(otherDomainUrl, StringComparison.InvariantCultureIgnoreCase);
    }
}
