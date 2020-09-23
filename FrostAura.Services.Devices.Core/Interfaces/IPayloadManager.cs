using FrostAura.Services.Devices.Shared.Models;
using System.Collections.Generic;

namespace FrostAura.Services.Devices.Core.Interfaces
{
    /// <summary>
    /// Manager for parsing and transforming payloads.
    /// </summary>
    public interface IPayloadManager
    {
        /// <summary>
        /// Convert a given payload to a dictionary of mapped attributes based on config.
        /// </summary>
        /// <param name="payload">Original payload.</param>
        /// <returns>Mapped dictionary payload.</returns>
        (string identifier, IDictionary<string, string> attributes) ToMappedDictionary(object payload, IList<AttributeMappingConfig> mappings);
    }
}