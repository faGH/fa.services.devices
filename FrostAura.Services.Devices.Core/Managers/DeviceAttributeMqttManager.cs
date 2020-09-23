using FrostAura.Libraries.Core.Extensions.Validation;
using FrostAura.Services.Devices.Core.Interfaces;
using FrostAura.Services.Devices.Data.Interfaces;
using FrostAura.Services.Devices.Shared.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FrostAura.Services.Devices.Core.Managers
{
    /// <summary>
    /// MQTT manager for grabbing device attributes from a remote server.
    /// See: https://github.com/chkr1011/MQTTnet and https://www.hivemq.com/blog/how-to-get-started-with-mqtt/
    /// </summary>
    public class DeviceAttributeMqttManager : IMqttManager, IDisposable
    {
        /// <summary>
        /// Configuration resource.
        /// </summary>
        private readonly MqttAttributeProviderConfig _config;
        /// <summary>
        /// Engine to perform payload related tasks.
        /// </summary>
        private readonly IPayloadManager _payloadManager;
        /// <summary>
        /// Instance logger.
        /// </summary>
        private readonly ILogger _logger;
        /// <summary>
        /// Devices manager.
        /// </summary>
        private readonly IDeviceManager _deviceManager;
        /// <summary>
        /// Mqtt resource accessor.
        /// </summary>
        private readonly IMqttResource _mqttResource;

        /// <summary>
        /// Constructor to provide dependencies.
        /// </summary>
        /// <param name="configurationResource">Configuration resource.</param>
        /// <param name="mqttResource">Mqtt resource accessor.</param>
        /// <param name="payloadEngine">Manager to perform payload related tasks.</param>
        /// <param name="deviceManager">Devices manager.</param>
        /// <param name="logger">Instance logger.</param>
        public DeviceAttributeMqttManager(IConfigurationResource configurationResource, IMqttResource mqttResource, IPayloadManager payloadManager, IDeviceManager deviceManager, ILogger<DeviceAttributeMqttManager> logger)
        {
            _config = configurationResource
                .ThrowIfNull(nameof(configurationResource))
                .GetMqttAttributeProviders();
            _mqttResource = mqttResource
                .ThrowIfNull(nameof(mqttResource));
            _payloadManager = payloadManager
                .ThrowIfNull(nameof(payloadManager));
            _deviceManager = deviceManager
                .ThrowIfNull(nameof(deviceManager));
            _logger = logger
                .ThrowIfNull(nameof(logger));
        }

        /// <summary>
        /// Initialize the manager.
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        public async Task InitializeAsync(CancellationToken token)
        {
            _mqttResource.OnMessage += async p => await HandleIncomingMessageAsync(p);

            _logger.LogDebug($"Registered event handler for incoming MQTT messages. Initiating the MQTT resource now.");
            await _mqttResource.InitializeAsync(token);
        }

        /// <summary>
        /// Event handler for when a new message is received.
        /// </summary>
        /// <param name="payload">Raw stringified payload.</param>
        private async Task HandleIncomingMessageAsync(string payload)
        {
            if (string.IsNullOrWhiteSpace(payload)) return;

            // Get mapped attributes from payload, based on config.
            (var identifier, var attributes) = _payloadManager.ToMappedDictionary(payload, _config.Mappings);

            // Add device attributes.
            await  _deviceManager.AddDeviceAttributesAsync(identifier, attributes);
            _logger.LogDebug($"{attributes.Count} attributes logged for device '{identifier}'.");
        }

        /// <summary>
        /// Cleaning up of resources.
        /// </summary>
        public void Dispose()
        {
            _mqttResource.OnMessage -= async p => await HandleIncomingMessageAsync(p);
            _logger.LogDebug($"Unregistered event handler for incoming MQTT messages.");
        }
    }
}
