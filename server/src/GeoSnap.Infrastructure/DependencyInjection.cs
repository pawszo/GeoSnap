using Microsoft.EntityFrameworkCore;
using GeoSnap.Application.Interfaces;
using GeoSnap.Infrastructure.Context;
using GeoSnap.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using GeoSnap.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace GeoSnap.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration["CacheConnection"];
            options.InstanceName = configuration["geosnap"];
        });
        var dbConnString = configuration.GetConnectionString("postgres");
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddDbContext<ApplicationDbContext>((sp, builder) =>
                   builder.UseNpgsql(dbConnString));
        services.AddScoped<INetworkAddressStoringService, NetworkAddressStoringService>();
        services.AddKeyedScoped<IGeoLocationDataProvider, IpStackService>("main");
        services.AddKeyedScoped<IGeoLocationDataProvider, IpifyService>("alternative");
        services.AddScoped<IGeoLocationService, GeoLocationService>();
        services.AddScoped<INetworkAddressRepository, NetworkAddressRepository>();
        services.AddTransient<IDnsResolvingService, DnsResolvingService>();
        //services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

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
