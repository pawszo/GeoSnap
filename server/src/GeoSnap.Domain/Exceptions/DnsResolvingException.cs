namespace GeoSnap.Domain.Exceptions;
public class DnsResolvingException(string domainUrl) : Exception($"Failed to resolve IP for domain {domainUrl}") { }
