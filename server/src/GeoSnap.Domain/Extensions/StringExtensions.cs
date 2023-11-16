using GeoSnap.Domain.Enums;
using GeoSnap.Domain.Constants;
using System.Text.RegularExpressions;

namespace GeoSnap.Domain.Extensions;
public static class StringExtensions
{
    public static bool IsValidNetworkAddress(this string address) => address.TryGetValidDomainUrl(out _) || address.TryGetValidIp(out _, out _);

    public static bool TryGetValidIp(this string url, out string ip, out ProtocolVersion version)
    {
        var matchV4 = Regex.Match(url, RegexPatterns.IpV4, RegexOptions.IgnoreCase);
        if (matchV4.Success)
        {
            ip = matchV4.Groups[1].Value.ToLower();
            version = ProtocolVersion.IPv4;
            return true;
        }

        var matchV6 = Regex.Match(url, RegexPatterns.IpV6, RegexOptions.IgnoreCase);
        if (matchV4.Success)
        {
            ip = matchV4.Groups[1].Value.ToLower();
            version = ProtocolVersion.IPv6;
            return true;
        }

        ip = string.Empty;
        version = ProtocolVersion.Unknown;
        return false;
    }

    public static bool TryGetValidDomainUrl(this string url, out string domainUrl)
    {
        var match = Regex.Match(url, RegexPatterns.DomainUrl, RegexOptions.IgnoreCase);
        if (match.Success)
        {
            domainUrl = match.Groups[2].Value.ToLower();
            return true;
        }

        domainUrl = string.Empty;
        return false;
    }
}
