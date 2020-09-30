using FrostAura.Services.Devices.Data.GraphQl.Queries;
using FrostAura.Services.Devices.Data.Interfaces;
using FrostAura.Services.Devices.Data.Resources;
using FrostAura.Services.Devices.Shared.Models;
using HotChocolate;
using HotChocolate.Execution.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Reflection;

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
            var devicesConnectionString = config.GetConnectionString("DevicesDbConnection");
            var migrationsAssembly = typeof(DevicesDbContext).GetTypeInfo().Assembly.GetName().Name;

            return services
                .AddDbContext<DevicesDbContext>(config =>
                {
                    config.UseSqlServer(devicesConnectionString);
                })
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
                .AddGraphQL(SchemaBuilder
                    .New()
                    .AddQueryType<ApplicationQuery>()
                    .Create(),
                    new QueryExecutionOptions { ForceSerialExecution = true })
                .AddSingleton<IConfigurationResource, OptionsConfigurationResource>()
                .AddSingleton<IMqttResource, MqttDotNetResource>()
                .AddSingleton<IDeviceResource, DeviceResource>();
        }
    }
}
