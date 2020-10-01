using FrostAura.Libraries.Core.Extensions.Validation;
using FrostAura.Services.Devices.Data.GraphQl.Requests;
using FrostAura.Services.Devices.Data.Interfaces;
using FrostAura.Services.Devices.Data.Models.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FrostAura.Services.Devices.Data.GraphQl
{
    /// <summary>
    /// GraphQL shema mutations.
    /// </summary>
    public class Mutation
    {
        /// <summary>
        /// Devices resource.
        /// </summary>
        private readonly IDeviceResource _deviceResource;

        /// <summary>
        /// Inject dependencies.
        /// </summary>
        /// <param name="deviceResource">Devices resource.</param>
        public Mutation(IDeviceResource deviceResource)
        {
            _deviceResource = deviceResource
                .ThrowIfNull(nameof(deviceResource));
        }

        /// <summary>
        /// Create or update a device.
        /// </summary>
        /// <param name="request">Device to upsert.</param>
        /// <returns>Upserted device context.</returns>
        public async Task<Device> UpsertDeviceAsync(DeviceRequest request, CancellationToken token)
        {
            request.ThrowIfNull(nameof(request));

            var response = await _deviceResource.UpsertAsync(new Device
            {
                Id = request.Id,
                Name = request.Name
            }, d => d.Id == request.Id, token);

            return response;
        }

        /// <summary>
        /// Upsert attribute values for a device.
        /// </summary>
        /// <param name="request">Attributes to upsert.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Whether the operation succeeded.</returns>
        public async Task<bool> UpsertAttributeValuesForDevice(DeviceAttributeRequest request, CancellationToken token)
        {
            if(request.DeviceId <= 0) throw new ArgumentException("A valid and existing device id is required.", nameof(request.Attributes));
            if (!request.Attributes.Any()) throw new ArgumentException("One or more attributes are required.", nameof(request.Attributes));

            var device = await _deviceResource.UpsertAsync(new Device { Id = request.DeviceId }, d => d.Id == request.DeviceId, token);

            await _deviceResource.AddDeviceAttributesAsync(device.ThrowIfNull(nameof(device)), request
                .Attributes
                .ToDictionary(a => a.Key, a => a.Value), token);

            return true;
        }
    }
}
