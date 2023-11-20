using Microsoft.EntityFrameworkCore;
using GeoSnap.Application.Interfaces;
using GeoSnap.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using GeoSnap.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace GeoSnap.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
                   options.UseInMemoryDatabase("TEST_DATABASE"));
        //TODO - Add real database

        services.AddScoped<INetworkAddressStoringService, NetworkAddressStoringService>();
        services.AddKeyedScoped<IGeoLocationDataProvider, IpStackService>("main");
        services.AddKeyedScoped<IGeoLocationDataProvider, IpifyService>("alternative");
        services.AddScoped<IGeoLocationService, GeoLocationService>();
        services.AddScoped<INetworkAddressRepository, NetworkAddressRepository>();
        services.AddTransient<IDnsResolvingService, DnsResolvingService>();

        services.AddHttpClient("IpStack", client =>
        {
            client.BaseAddress = new Uri(configuration["BaseUrl:IpStack"]);
        });

        services.AddHttpClient("Ipify", client =>
        {
            client.BaseAddress = new Uri(configuration["BaseUrl:Ipify"]);

        });
        return services;
    }
}
