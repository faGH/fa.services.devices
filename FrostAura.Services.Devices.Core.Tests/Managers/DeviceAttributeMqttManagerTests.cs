using FrostAura.Services.Devices.Core.Interfaces;
using FrostAura.Services.Devices.Core.Managers;
using FrostAura.Services.Devices.Data.Interfaces;
using FrostAura.Services.Devices.Shared.Models;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace FrostAura.Services.Devices.Core.Tests.Managers
{
    public class DeviceAttributeMqttManagerTests
    {
        [Fact]
        public void Constructor_WithInvalidConfigResource_ShouldThrow()
        {
            IConfigurationResource configResource = null;
            IMqttResource mqttResource = Substitute.For<IMqttResource>();
            IPayloadManager payloadManager = Substitute.For<IPayloadManager>();
            IDeviceManager deviceManager = Substitute.For<IDeviceManager>();
            ILogger<DeviceAttributeMqttManager> logger = Substitute.For<ILogger<DeviceAttributeMqttManager>>();

            Assert.Throws<ArgumentNullException>(() => new DeviceAttributeMqttManager(configResource, mqttResource, payloadManager, deviceManager, logger));
        }

        [Fact]
        public void Constructor_WithInvalidMqttResource_ShouldThrow()
        {
            IConfigurationResource configResource = Substitute.For<IConfigurationResource>();
            IMqttResource mqttResource = null;
            IPayloadManager payloadManager = Substitute.For<IPayloadManager>();
            IDeviceManager deviceManager = Substitute.For<IDeviceManager>();
            ILogger<DeviceAttributeMqttManager> logger = Substitute.For<ILogger<DeviceAttributeMqttManager>>();

            Assert.Throws<ArgumentNullException>(() => new DeviceAttributeMqttManager(configResource, mqttResource, payloadManager, deviceManager, logger));
        }

        [Fact]
        public void Constructor_WithInvalidPayloadManager_ShouldThrow()
        {
            IConfigurationResource configResource = Substitute.For<IConfigurationResource>();
            IMqttResource mqttResource = Substitute.For<IMqttResource>();
            IPayloadManager payloadManager = null;
            IDeviceManager deviceManager = Substitute.For<IDeviceManager>();
            ILogger<DeviceAttributeMqttManager> logger = Substitute.For<ILogger<DeviceAttributeMqttManager>>();

            Assert.Throws<ArgumentNullException>(() => new DeviceAttributeMqttManager(configResource, mqttResource, payloadManager, deviceManager, logger));
        }

        [Fact]
        public void Constructor_WithInvalidDeviceManager_ShouldThrow()
        {
            IConfigurationResource configResource = Substitute.For<IConfigurationResource>();
            IMqttResource mqttResource = Substitute.For<IMqttResource>();
            IPayloadManager payloadManager = Substitute.For<IPayloadManager>();
            IDeviceManager deviceManager = null;
            ILogger<DeviceAttributeMqttManager> logger = Substitute.For<ILogger<DeviceAttributeMqttManager>>();

            Assert.Throws<ArgumentNullException>(() => new DeviceAttributeMqttManager(configResource, mqttResource, payloadManager, deviceManager, logger));
        }

        [Fact]
        public void Constructor_WithInvalidLogger_ShouldThrow()
        {
            IConfigurationResource configResource = Substitute.For<IConfigurationResource>();
            IMqttResource mqttResource = Substitute.For<IMqttResource>();
            IPayloadManager payloadManager = Substitute.For<IPayloadManager>();
            IDeviceManager deviceManager = Substitute.For<IDeviceManager>();
            ILogger<DeviceAttributeMqttManager> logger = null;

            Assert.Throws<ArgumentNullException>(() => new DeviceAttributeMqttManager(configResource, mqttResource, payloadManager, deviceManager, logger));
        }

        [Fact]
        public void Constructor_WithValidParams_ShouldConstruct()
        {
            IConfigurationResource configResource = Substitute.For<IConfigurationResource>();
            IMqttResource mqttResource = Substitute.For<IMqttResource>();
            IPayloadManager payloadManager = Substitute.For<IPayloadManager>();
            IDeviceManager deviceManager = Substitute.For<IDeviceManager>();
            ILogger<DeviceAttributeMqttManager> logger = Substitute.For<ILogger<DeviceAttributeMqttManager>>();

            var actual = new DeviceAttributeMqttManager(configResource, mqttResource, payloadManager, deviceManager, logger);

            Assert.NotNull(actual);
        }

        [Fact]
        public async Task InitializeAsync_WithValidParams_ShouldCallInitializeAsyncOnResource()
        {
            var configResource = Substitute.For<IConfigurationResource>();
            var mqttResource = Substitute.For<IMqttResource>();
            var payloadManager = Substitute.For<IPayloadManager>();
            var deviceManager = Substitute.For<IDeviceManager>();
            var logger = Substitute.For<ILogger<DeviceAttributeMqttManager>>();
            var actual = new DeviceAttributeMqttManager(configResource, mqttResource, payloadManager, deviceManager, logger);

            await actual.InitializeAsync(CancellationToken.None);

            mqttResource
                .Received()
                .InitializeAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public void HandleIncomingMessageAsync_WithInvalidPayload_ShouldNotCallPayloadManagerToMappedDictionary()
        {
            var configResource = Substitute.For<IConfigurationResource>();
            var mqttResource = Substitute.For<IMqttResource>();
            var payloadManager = Substitute.For<IPayloadManager>();
            var deviceManager = Substitute.For<IDeviceManager>();
            var logger = Substitute.For<ILogger<DeviceAttributeMqttManager>>();
            var instance = new DeviceAttributeMqttManager(configResource, mqttResource, payloadManager, deviceManager, logger);
            var payload = string.Empty;

            var actual = instance
                .GetType()
                .GetMethod("HandleIncomingMessageAsync", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(instance, new[] { payload });

            payloadManager
                .DidNotReceive()
                .ToMappedDictionary(Arg.Any<object>(), Arg.Any<List<AttributeMappingConfig>>());
        }

        [Fact]
        public void HandleIncomingMessageAsync_WithValidPayload_ShouldCallPayloadManagerToMappedDictionary()
        {
            var configResource = Substitute.For<IConfigurationResource>();
            var mqttResource = Substitute.For<IMqttResource>();
            var payloadManager = Substitute.For<IPayloadManager>();
            var deviceManager = Substitute.For<IDeviceManager>();
            var logger = Substitute.For<ILogger<DeviceAttributeMqttManager>>();
            var identifier = "test";
            var config = new MqttAttributeProviderConfig 
            {
            };
            var payload = "{ hello: 1234 }";

            configResource
                .GetMqttAttributeProviders()
                .Returns(config);
            payloadManager
                .ToMappedDictionary(Arg.Any<object>(), Arg.Any<List<AttributeMappingConfig>>())
                .Returns((identifier, new Dictionary<string, string>()));

            var instance = new DeviceAttributeMqttManager(configResource, mqttResource, payloadManager, deviceManager, logger);

            var actual = instance
                .GetType()
                .GetMethod("HandleIncomingMessageAsync", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(instance, new[] { payload });

            payloadManager
                .Received()
                .ToMappedDictionary(Arg.Is<object>(payload), Arg.Any<List<AttributeMappingConfig>>());
        }

        [Fact]
        public void HandleIncomingMessageAsync_WithMappingsFromPayloadManager_ShouldCallDeviceManagerAddDeviceAttributesAsync()
        {
            var configResource = Substitute.For<IConfigurationResource>();
            var mqttResource = Substitute.For<IMqttResource>();
            var payloadManager = Substitute.For<IPayloadManager>();
            var deviceManager = Substitute.For<IDeviceManager>();
            var logger = Substitute.For<ILogger<DeviceAttributeMqttManager>>();
            var identifier = "test";
            var value = "someValue";
            var config = new MqttAttributeProviderConfig
            {
            };
            var payload = "{ hello: 1234 }";

            configResource
                .GetMqttAttributeProviders()
                .Returns(config);
            payloadManager
                .ToMappedDictionary(Arg.Any<object>(), Arg.Any<List<AttributeMappingConfig>>())
                .Returns((identifier, new Dictionary<string, string>
                {
                    { identifier, value }
                }));

            var instance = new DeviceAttributeMqttManager(configResource, mqttResource, payloadManager, deviceManager, logger);

            var actual = instance
                .GetType()
                .GetMethod("HandleIncomingMessageAsync", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(instance, new[] { payload });

            deviceManager
                .Received()
                .AddDeviceAttributesAsync(identifier, Arg.Is<IDictionary<string, string>>(d => d.Any() && d.First().Key == identifier && d.First().Value == value));
        }
    }
}
