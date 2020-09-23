using FrostAura.Libraries.Core.Extensions.Validation;
using FrostAura.Services.Devices.Data.Interfaces;
using FrostAura.Services.Devices.Shared.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace FrostAura.Services.Devices.Data.Resources
{
    /// <summary>
    /// Configuration resource that uses options in the back-end.
    /// </summary>
    public class OptionsConfigurationResource : IConfigurationResource
    {
        /// <summary>
        /// Applications config MQTT configuration.
        /// </summary>
        public readonly MqttAttributeProviderConfig _mqttProvidersConfiguration;

        /// <summary>
        /// Constructor to provide dependencies.
        /// </summary>
        /// <param name="options">Applications config MQTT configuration.</param>
        public OptionsConfigurationResource(IOptions<List<MqttAttributeProviderConfig>> options)
        {
            _mqttProvidersConfiguration = options
                .ThrowIfNull(nameof(options))
                .Value
                .ThrowIfNull(nameof(options.Value))
                .First();
        }

        /// <summary>
        /// Get the MQTT provider configuration.
        /// </summary>
        /// <returns>MQTT provider configuration.</returns>
        public MqttAttributeProviderConfig GetMqttAttributeProviders()
        {
            return _mqttProvidersConfiguration;
        }
    }
}
