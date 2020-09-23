using FrostAura.Services.Devices.Shared.Models;

namespace FrostAura.Services.Devices.Data.Interfaces
{
    /// <summary>
    /// Configuration resource accessor.
    /// </summary>
    public interface IConfigurationResource
    {
        /// <summary>
        /// Get the MQTT provider configuration.
        /// </summary>
        /// <returns>MQTT provider configuration.</returns>
        MqttAttributeProviderConfig GetMqttAttributeProviders();
    }
}
