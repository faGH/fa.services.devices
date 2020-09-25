using FrostAura.Libraries.Core.Extensions.Validation;
using FrostAura.Services.Devices.Data.Interfaces;
using FrostAura.Services.Devices.Shared.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        /// Instance logger.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor to provide dependencies.
        /// </summary>
        /// <param name="configurationResource">Configuration resource.</param>
        /// <param name="devicesDbContext">Devices DB context.</param>
        /// <param name="logger">Instance logger.</param>
        public DeviceResource(IConfigurationResource configurationResource, IServiceScopeFactory scopeFactory, ILogger<DeviceResource> logger)
        {
            configurationResource
                .ThrowIfNull(nameof(configurationResource))
                .GetMqttAttributeProviders();
            _scopeFactory = scopeFactory
                .ThrowIfNull(nameof(scopeFactory));
            _logger = logger
                .ThrowIfNull(nameof(logger));
        }

        /// <summary>
        /// Add or update a device.
        /// </summary>
        /// <param name="device">Device to upsert.</param>
        /// <returns></returns>
        public async Task<Device> UpsertAsync(Device device)
        {
            device
                .Name
                .ThrowIfNullOrWhitespace(nameof(device.Name));

            using(var scope = _scopeFactory.CreateScope())
            {
                var db = scope
                    .ServiceProvider
                    .GetService<DevicesDbContext>();
                var dbDevice = await db
                    .Devices
                    .AsNoTracking()
                    .FirstOrDefaultAsync(d => d.Name == device.Name);

                if(dbDevice != null)
                {
                    device.Id = dbDevice.Id;
                    db.Devices.Update(device);
                    _logger.LogDebug($"Device with name '{dbDevice.Name}' already exists. Using it.");
                }
                else
                {
                    db.Devices.Add(device);
                    _logger.LogDebug($"Device with name '{device.Name}' does not yet exists. Creating it.");
                }

                await db.SaveChangesAsync();

                return device;
            }
        }

        /// <summary>
        /// Add device attributes.
        /// </summary>
        /// <param name="deviceName">Unique device name / identifier.</param>
        /// <param name="attributes">Key value collection of attribute names and values.</param>
        /// <returns></returns>
        public async Task AddDeviceAttributesAsync(string deviceName, IDictionary<string, string> attributes)
        {
            // TODO: Migrate to AttributeResource.
            using (var scope = _scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<DevicesDbContext>();
                var device = await db.Devices.FirstAsync(d => d.Name == deviceName);

                _logger.LogDebug($"Inserting {attributes.Count} attribute(s) for device with name '{device.Name}'.");

                foreach (var attr in attributes)
                {
                    if (string.IsNullOrWhiteSpace(attr.Value)) continue;

                    var attribute = await db.Attributes.FirstOrDefaultAsync(a => a.Name == attr.Key);

                    if(attribute == null)
                    {
                        _logger.LogDebug($"Creating new attribute with name '{attr.Key}'.");

                        attribute = new Attribute { Name = attr.Key };
                        
                        db.Attributes.Add(attribute);

                        await db.SaveChangesAsync();
                    }

                    _logger.LogDebug($"Capturing value for attribute with name '{attr.Key}': '{attr.Value}'");

                    attribute.DeviceAttributes.Add(new DeviceAttribute { DeviceId = device.Id, Value = attr.Value });

                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
