using FrostAura.Libraries.Core.Extensions.Validation;
using FrostAura.Services.Devices.Data.Interfaces;
using FrostAura.Services.Devices.Shared.Models;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Client.Subscribing;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FrostAura.Services.Devices.Data.Resources
{
    /// <summary>
    /// MQTT.Net resource.
    /// See: https://github.com/chkr1011/MQTTnet and https://www.hivemq.com/blog/how-to-get-started-with-mqtt/
    /// </summary>
    public class MqttDotNetResource : IMqttResource, IDisposable
    {
        /// <summary>
        /// Event emitter for when a new message is received.
        /// <param type="string">Stringified payload.</param>
        /// </summary>
        public event Action<string> OnMessage;
        /// <summary>
        /// Configuration resource.
        /// </summary>
        private readonly MqttAttributeProviderConfig _config;
        /// <summary>
        /// Instance logger.
        /// </summary>
        private readonly ILogger _logger;
        /// <summary>
        /// MQTT client.
        /// </summary>
        private IMqttClient _client;

        /// <summary>
        /// Constructor to provide dependencies.
        /// </summary>
        /// <param name="configurationResource">Configuration resource.</param>
        /// <param name="logger">Instance logger.</param>
        public MqttDotNetResource(IConfigurationResource configurationResource, ILogger<MqttDotNetResource> logger)
        {
            _config = configurationResource
                .ThrowIfNull(nameof(configurationResource))
                .GetMqttAttributeProviders();
            _logger = logger
                .ThrowIfNull(nameof(logger));
        }

        /// <summary>
        /// Initialize the manager.
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        public async Task InitializeAsync(CancellationToken token)
        {
            try
            {
                var factory = new MqttFactory();
                var clientId = $"frostaura.services.devices.{Guid.NewGuid()}";
                var options = new MqttClientOptionsBuilder()
                    .WithClientId(clientId)
                    .WithCredentials(_config.Username, _config.Password)
                    .WithTcpServer(_config.Server, _config.Port)
                    .WithCleanSession()
                    .Build();

                _client?.Dispose();

                _client = factory.CreateMqttClient();
                _client.ConnectedHandler = new MqttClientConnectedHandlerDelegate(async args =>
                {
                    var topicFilter = new MqttTopicFilterBuilder()
                        .WithTopic(_config.Topic)
                        .Build();
                    var topic = new MqttClientSubscribeOptionsBuilder()
                        .WithTopicFilter(topicFilter)
                        .Build();

                    _logger.LogDebug($"Connected to MQTT server '{_config.Server}'. Subscribing to topic '{_config.Topic}'.");
                    await _client.SubscribeAsync(topic, token);
                });
                _client.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(async args =>
                {
                    _logger.LogWarning($"Disconnected from MQTT server '{_config.Server}'. Attempting to reconnect now.");
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    await InitializeAsync(token);
                });
                _client.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(HandleIncomingMessage);

                await _client.ConnectAsync(options, token);
                _logger.LogDebug("MqttDotNet client initialized.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to initialize MqttDotNet client: {e.Message}", e);
            }
        }

        /// <summary>
        /// Event handler for when a new message is received.
        /// </summary>
        /// <param name="args">Event arguments.</param>
        private void HandleIncomingMessage(MqttApplicationMessageReceivedEventArgs args)
        {
            var payload = Encoding
                .UTF8
                .GetString(args.ApplicationMessage.Payload);

            try
            {
                _logger.LogDebug($"Incoming message received from MQTT server '{_config.Server}': '{payload}'");

                OnMessage?.Invoke(payload);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to parse incoming payload '{payload}': {e.Message}", e);
            }
        }

        /// <summary>
        /// Clean up resources.
        /// </summary>
        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
