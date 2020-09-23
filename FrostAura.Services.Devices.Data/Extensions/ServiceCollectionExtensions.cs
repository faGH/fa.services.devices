using FrostAura.Services.Devices.Data.Interfaces;
using FrostAura.Services.Devices.Data.Resources;
using FrostAura.Services.Devices.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace FrostAura.Services.Devices.Data.Extensions
{
    /// <summary>
    /// Extensions for IServiceCollection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add all required application resource services and config to the DI container.
        /// </summary>
        /// <param name="services">Application services collection.</param>
        /// <param name="config">Configuration for the application.</param>
        /// <returns>Application services collection.</returns>
        public static IServiceCollection AddFrostAuraResources(this IServiceCollection services, IConfiguration config)
        {
            return services
                .AddConfig(config)
                .AddServices();
        }

        /// <summary>
        /// Add all required application resources configuration to the DI container.
        /// </summary>
        /// <param name="services">Application services collection.</param>
        /// <returns>Application services collection.</returns>
        private static IServiceCollection AddConfig(this IServiceCollection services, IConfiguration config)
        {
            return services
                .AddOptions()
                .Configure<List<MqttAttributeProviderConfig>>(config.GetSection("MqttAttributeProviders"));
        }

        /// <summary>
        /// Add all required application resource services to the DI container.
        /// </summary>
        /// <param name="services">Application services collection.</param>
        /// <returns>Application services collection.</returns>
        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddSingleton<IConfigurationResource, OptionsConfigurationResource>()
                .AddSingleton<IMqttResource, MqttDotNetResource>()
                .AddSingleton<IDeviceResource, DeviceResource>();
        }
    }
}
