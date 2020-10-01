using FrostAura.Libraries.Core.Extensions.Validation;
using FrostAura.Services.Devices.Data.Interfaces;
using FrostAura.Services.Devices.Data.Models.Entities;
using HotChocolate.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Attribute = FrostAura.Services.Devices.Data.Models.Entities.Attribute;

namespace FrostAura.Services.Devices.Data.Resources
{
    /// <summary>
    /// Devices resource.
    /// </summary>
    public class DeviceResource : IDeviceResource
    {
        /// <summary>
        /// Service scope factory.
        /// </summary>
        private readonly IServiceScopeFactory _scopeFactory;
        /// <summary>
        /// GraphQL event emitter.
        /// </summary>
        private readonly ITopicEventSender _eventEmitter;
        /// <summary>
        /// Instance logger.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor to provide dependencies.
        /// </summary>
        /// <param name="configurationResource">Configuration resource.</param>
        /// <param name="devicesDbContext">Devices DB context.</param>
        /// <param name="scopeFactory">Application scope factory.</param>
        /// <param name="eventEmitter">GraphQL event emitter.</param>
        /// <param name="logger">Instance logger.</param>
        public DeviceResource(IConfigurationResource configurationResource, IServiceScopeFactory scopeFactory, ITopicEventSender eventEmitter, ILogger<DeviceResource> logger)
        {
            configurationResource
                .ThrowIfNull(nameof(configurationResource))
                .GetMqttAttributeProviders();
            _scopeFactory = scopeFactory
                .ThrowIfNull(nameof(scopeFactory));
            _eventEmitter = eventEmitter
                .ThrowIfNull(nameof(eventEmitter));
            _logger = logger
                .ThrowIfNull(nameof(logger));
        }

        /// <summary>
        /// Add or update a device by its selector.
        /// </summary>
        /// <param name="device">Device to upsert.</param>
        /// <param name="identifierExpression">Selector for the identifier to use to do a DB comparison.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Device context.</returns>
        public async Task<Device> UpsertAsync(Device device, Expression<Func<Device, bool>> identifierExpression, CancellationToken token)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var db = scope
                    .ServiceProvider
                    .GetService<DevicesDbContext>();
                var dbDevice = await db
                    .Devices
                    .AsNoTracking()
                    .FirstOrDefaultAsync(identifierExpression, token);

                if (dbDevice != null)
                {
                    if (string.IsNullOrWhiteSpace(device.Name) || dbDevice.Name == device.Name) return dbDevice;

                    // All future fields that may require updating, should go beneith here.
                    dbDevice.Name = device.Name;

                    db.Devices.Update(dbDevice);
                    device = dbDevice;
                    _logger.LogDebug("Device already exists. Using it.");
                }
                else
                {
                    db.Devices.Add(device);
                    _logger.LogDebug("Device does not yet exists. Creating it.");
                }

                await db.SaveChangesAsync(token);
                await _eventEmitter.SendAsync("devices", device);

                return device;
            }
        }

        /// <summary>
        /// Add device attributes.
        /// </summary>
        /// <param name="device">Device context.</param>
        /// <param name="attributes">Key value collection of attribute names and values.</param>
        /// <param name="token">Cancellation token.</param>
        public async Task AddDeviceAttributesAsync(Device device, IDictionary<string, string> attributes, CancellationToken token)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<DevicesDbContext>();

                _logger.LogDebug($"Inserting {attributes.Count} attribute(s) for device with name '{device.Name}'.");

                foreach (var attr in attributes)
                {
                    if (string.IsNullOrWhiteSpace(attr.Value)) continue;

                    var attribute = await db.Attributes.FirstOrDefaultAsync(a => a.Name == attr.Key, token);

                    if (attribute == null)
                    {
                        _logger.LogDebug($"Creating new attribute with name '{attr.Key}'.");

                        attribute = new Attribute { Name = attr.Key };

                        db.Attributes.Add(attribute);

                        await db.SaveChangesAsync(token);
                    }

                    _logger.LogDebug($"Capturing value for attribute with name '{attr.Key}': '{attr.Value}'");

                    var deviceAttr = new DeviceAttribute { DeviceId = device.Id, Value = attr.Value };

                    attribute.DeviceAttributes.Add(deviceAttr);

                    await db.SaveChangesAsync(token);
                    await _eventEmitter.SendAsync($"device.{device.Id}.attributes", device);
                }
            }
        }
    }
}
