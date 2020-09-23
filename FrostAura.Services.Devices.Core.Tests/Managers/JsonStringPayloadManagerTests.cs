using FrostAura.Services.Devices.Core.Managers;
using FrostAura.Services.Devices.Shared.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace FrostAura.Services.Devices.Core.Tests.Managers
{
    public class JsonStringPayloadManagerTests
    {
        [Fact]
        public void Constructor_WithInvalidLogger_ShouldThrow()
        {
            ILogger<JsonStringPayloadManager> logger = null;

            Assert.Throws<ArgumentNullException>(() => new JsonStringPayloadManager(logger));
        }

        [Fact]
        public void Constructor_WithValidLogger_ShouldConstruct()
        {
            var logger = Substitute.For<ILogger<JsonStringPayloadManager>>();
            var actual = new JsonStringPayloadManager(logger);

            Assert.NotNull(actual);
        }

        [Fact]
        public void NormalizeStringPayload_WithBadPayload_ShouldNormalizePayload()
        {
            var payload = "{\"DeviceId\":\"862877031027087\",\"MesId\":\"103\",\"DateTime\":2020-09-22 15:45:55,\"LonLat\":[24.466528,-29.781727],\"Speed\":0.0,\"Direction\":96.59}";
            var expected = "{\"DeviceId\":\"862877031027087\",\"MesId\":\"103\",\"DateTime\":\"2020-09-22 15:45:55\",\"LonLat\":[24.466528,-29.781727],\"Speed\":0.0,\"Direction\":96.59}";
            var instance = GetInstance();

            var actual = instance
                .GetType()
                .GetMethod("NormalizeStringPayload", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(instance, new[] { payload });

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NormalizeStringPayload_WithValidPayloadButNoMappings_ShouldReturnEmptyResponse()
        {
            var payload = "{\"DeviceId\":\"862877031027087\",\"MesId\":\"103\",\"DateTime\":2020-09-22 15:45:55,\"LonLat\":[24.466528,-29.781727],\"Speed\":0.0,\"Direction\":96.59}";
            var instance = GetInstance();
            var mappings = new List<AttributeMappingConfig>();

            (var identifier, var attributes) = instance.ToMappedDictionary(payload, mappings);

            Assert.True(string.IsNullOrWhiteSpace(identifier));
            Assert.Equal(mappings.Count, attributes.Count);
        }

        [Fact]
        public void NormalizeStringPayload_WithValidPayload_ShouldReturnCorrectIdentifier()
        {
            var deviceId = "862877031027087";
            var payload = "{\"DeviceId\":\"" + deviceId + "\",\"MesId\":\"103\",\"DateTime\":2020-09-22 15:45:55,\"LonLat\":[24.466528,-29.781727],\"Speed\":0.0,\"Direction\":96.59}";
            var instance = GetInstance();
            var mappings = new List<AttributeMappingConfig>
            {
                new AttributeMappingConfig { IsDeviceIdentifier = true, Source = "DeviceId" }
            };

            (var identifier, var attributes) = instance.ToMappedDictionary(payload, mappings);

            Assert.Equal(deviceId, identifier);
        }

        [Fact]
        public void NormalizeStringPayload_WithValidPayload_ShouldReturnComplexAttributeMappings()
        {
            var payloadRaw = new
            {
                DeviceId = "862877031027087",
                MesId = "103",
                DateTime = "2020-09-22 15:45:55",
                LonLat = new[] { 24.466528, -29.781727 },
                Speed = 0.0,
                Direction = 96.59
            };
            var payload = JsonConvert.SerializeObject(payloadRaw);
            var instance = GetInstance();
            var mappings = new List<AttributeMappingConfig>
            {
                new AttributeMappingConfig { IsDeviceIdentifier = true, Source = "MesId", Destination = "Id" },
                new AttributeMappingConfig { Source = "LonLat[0]", Destination = "Lon" },
                new AttributeMappingConfig { Source = "LonLat[1]", Destination = "Lat" },
            };

            (var identifier, var attributes) = instance.ToMappedDictionary(payload, mappings);

            Assert.Equal(attributes.Count, mappings.Count);
            Assert.Equal(payloadRaw.MesId, attributes["Id"]);
            Assert.Equal(payloadRaw.LonLat[0].ToString(), attributes["Lon"]);
            Assert.Equal(payloadRaw.LonLat[1].ToString(), attributes["Lat"]);
        }

        [Fact]
        public void ToMappedDictionary_WithInvalidPayload_ShouldThrow()
        {
            var instance = GetInstance();

            var actual = Assert.Throws<ArgumentNullException>(() => instance.ToMappedDictionary(null, new List<AttributeMappingConfig>()));
        }

        private JsonStringPayloadManager GetInstance()
        {
            var logger = Substitute.For<ILogger<JsonStringPayloadManager>>();
            var instance = new JsonStringPayloadManager(logger);

            return instance;
        }
    }
}