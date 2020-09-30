using FrostAura.Services.Devices.Data.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FrostAura.Services.Devices.Data.Interfaces
{
    /// <summary>
    /// Device resource accessor.
    /// </summary>
    public interface IDeviceResource
    {
        /// <summary>
        /// Add or update a device.
        /// </summary>
        /// <param name="device">Device to upsert.</param>
        /// <returns></returns>
        Task<Device> UpsertAsync(Device device);
        /// <summary>
        /// Add device attributes.
        /// </summary>
        /// <param name="deviceName">Unique device name / identifier.</param>
        /// <param name="attributes">Key value collection of attribute names and values.</param>
        /// <returns></returns>
        Task AddDeviceAttributesAsync(string deviceName, IDictionary<string, string> attributes);
    }
}
