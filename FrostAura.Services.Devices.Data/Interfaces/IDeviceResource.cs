using FrostAura.Services.Devices.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace FrostAura.Services.Devices.Data.Interfaces
{
    /// <summary>
    /// Device resource accessor.
    /// </summary>
    public interface IDeviceResource
    {
        /// <summary>
        /// Add or update a device by its selector.
        /// </summary>
        /// <param name="device">Device to upsert.</param>
        /// <param name="identifierExpression">Selector for the identifier to use to do a DB comparison.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Device context.</returns>
        Task<Device> UpsertAsync(Device device, Expression<Func<Device, bool>> identifierExpression, CancellationToken token);
        /// <summary>
        /// Add device attributes.
        /// </summary>
        /// <param name="device">Device context.</param>
        /// <param name="attributes">Key value collection of attribute names and values.</param>
        /// <param name="token">Cancellation token.</param>
        Task AddDeviceAttributesAsync(Device device, IDictionary<string, string> attributes, CancellationToken token);
    }
}
