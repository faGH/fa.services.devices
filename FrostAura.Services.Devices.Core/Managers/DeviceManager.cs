using FrostAura.Libraries.Core.Extensions.Validation;
using FrostAura.Services.Devices.Core.Interfaces;
using FrostAura.Services.Devices.Data.Interfaces;
using FrostAura.Services.Devices.Data.Models.Entities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FrostAura.Services.Devices.Core.Managers
{
    /// <summary>
    /// Manager for devices.
    /// </summary>
    public class DeviceManager : IDeviceManager
    {
        /// <summary>
        /// Device resource accessor.
        /// </summary>
        private readonly IDeviceResource _deviceResource;
        /// <summary>
        /// Instance logger.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor to provide dependencies.
        /// </summary>
        /// <param name="deviceResource">Device resource accessor.</param>
        /// <param name="logger">Instance logger.</param>
        public DeviceManager(IDeviceResource deviceResource, ILogger<DeviceManager> logger)
        {
            _deviceResource = deviceResource
                .ThrowIfNull(nameof(deviceResource));
            _logger = logger
                .ThrowIfNull(nameof(logger));
        }

        /// <summary>
        /// Upsert a collection of attributes for a given device identifier.
        /// </summary>
        /// <param name="deviceName">Device identifier.</param>
        /// <param name="attributes">Device attributes.</param>
        /// <param name="token">Cancellation token.</param>
        public async Task AddDeviceAttributesAsync(string deviceName, IDictionary<string, string> attributes, CancellationToken token)
        {
            var device = await _deviceResource.UpsertAsync(new Device { Name = deviceName }, d => d.Name == deviceName, token);
            
            await _deviceResource.AddDeviceAttributesAsync(device, attributes, token);
        }
    }
}
