using System.Collections.Generic;
using System.Threading.Tasks;

namespace FrostAura.Services.Devices.Core.Interfaces
{
    /// <summary>
    /// Devices manager.
    /// </summary>
    public interface IDeviceManager
    {
        /// <summary>
        /// Upsert a collection of attributes for a given device identifier.
        /// </summary>
        /// <param name="identifier">Device identifier.</param>
        /// <param name="attributes">Device attributes.</param>
        Task AddDeviceAttributesAsync(string identifier, IDictionary<string, string> attributes);
    }
}
