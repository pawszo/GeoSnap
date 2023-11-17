using MediatR;
using System.Reflection;
using GeoSnap.Application.Common.Behaviours;
using Microsoft.Extensions.DependencyInjection;

namespace GeoSnap.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        });
        return services;
    }
}