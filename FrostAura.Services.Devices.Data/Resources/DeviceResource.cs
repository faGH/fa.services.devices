using FrostAura.Libraries.Core.Extensions.Validation;
using FrostAura.Services.Devices.Data.Interfaces;
using Microsoft.Extensions.Logging;
namespace FrostAura.Services.Devices.Data.Resources
{
    /// <summary>
    /// Devices resource.
    /// </summary>
    public class DeviceResource : IDeviceResource
    {
        /// <summary>
        /// Instance logger.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor to provide dependencies.
        /// </summary>
        /// <param name="configurationResource">Configuration resource.</param>
        /// <param name="logger">Instance logger.</param>
        public DeviceResource(IConfigurationResource configurationResource, ILogger<DeviceResource> logger)
        {
            configurationResource
                .ThrowIfNull(nameof(configurationResource))
                .GetMqttAttributeProviders();
            _logger = logger
                .ThrowIfNull(nameof(logger));
        }
    }
}
