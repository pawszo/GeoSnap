using GeoSnap.Domain.Constants;
using System.Text.RegularExpressions;

namespace GeoSnap.Domain.Extensions;
public static class StringExtensions
{
    public static bool IsValidNetworkAddress(this string address) => address.TryGetValidDomainUrl(out _) || address.TryGetValidIp(out _);

    public static bool TryGetValidIp(this string url, out string ip)
    {
        var match = Regex.Match(url, RegexPatterns.Ip, RegexOptions.IgnoreCase);
        if (match.Success)
        {
            ip = match.Groups[1].Value.ToLower();
            return true;
        }

        ip = string.Empty;
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
