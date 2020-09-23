using FrostAura.Services.Devices.Core.Managers;
using FrostAura.Services.Devices.Data.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using Xunit;

namespace FrostAura.Services.Devices.Core.Tests.Managers
{
    public class DeviceManagerTests
    {
        [Fact]
        public void Constructor_WithInvalidDeviceResource_ShouldThrow()
        {
            IDeviceResource deviceResource = null;
            ILogger<DeviceManager> logger = Substitute.For<ILogger<DeviceManager>>();

            Assert.Throws<ArgumentNullException>(() => new DeviceManager(deviceResource, logger));
        }

        [Fact]
        public void Constructor_WithInvalidLogger_ShouldThrow()
        {
            IDeviceResource deviceResource = Substitute.For<IDeviceResource>();
            ILogger<DeviceManager> logger = null;

            Assert.Throws<ArgumentNullException>(() => new DeviceManager(deviceResource, logger));
        }

        [Fact]
        public void Constructor_WithValidParams_ShouldConstruct()
        {
            var deviceResource = Substitute.For<IDeviceResource>();
            var logger = Substitute.For<ILogger<DeviceManager>>();

            var actual = new DeviceManager(deviceResource, logger);

            Assert.NotNull(actual);
        }
    }
}
