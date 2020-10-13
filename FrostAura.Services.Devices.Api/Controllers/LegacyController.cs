using FrostAura.Libraries.Core.Extensions.Validation;
using FrostAura.Services.Devices.Core.Interfaces;
using FrostAura.Services.Devices.Data.Interfaces;
using FrostAura.Services.Devices.Data.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FrostAura.Services.Devices.Api.Controllers
{
    /// <summary>
    /// Controller to handle legacy device API requests.
    /// </summary>
    [Route("api")]
    [ApiController]
    public class LegacyController : ControllerBase
    {
        /// <summary>
        /// Devices manager.
        /// </summary>
        private readonly IDeviceManager _deviceManager;
        /// <summary>
        /// Instance logger.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Provide dependencies.
        /// </summary>
        /// <param name="deviceManager">Devices manager.</param>
        /// <param name="logger">Instance logger.</param>
        public LegacyController(IDeviceManager deviceManager, ILogger<LegacyController> logger)
        {
            _deviceManager = deviceManager
                .ThrowIfNull(nameof(deviceManager));
            _logger = logger
                .ThrowIfNull(nameof(logger));
        }

        /// <summary>
        /// Capture and log a legacy device attribute.
        /// 
        /// E.g. /api/PP.MK3.003/-29DOT836907/24DOT522360
        /// </summary>
        /// <param name="deviceName">Device name.</param>
        /// <param name="lat">Latitude.</param>
        /// <param name="lng">Longitude.</param>
        /// <param name="token">Cancellation token.</param>
        [HttpGet("{deviceName}/{lat}/{lng}")]
        public async Task<IActionResult> CaptureLegacyDeviceAttributeAsync(string deviceName, string lat, string lng, CancellationToken token)
        {
            _logger.LogInformation($"Capturing legacy details: Name: '{deviceName}', Lat: {lat}, Lng: {lng}");

            var name = deviceName.ThrowIfNullOrWhitespace(nameof(deviceName));
            var parsedLat = lat.ThrowIfNullOrWhitespace(nameof(lat)).Replace("DOT", ".");
            var parsedLng = lng.ThrowIfNullOrWhitespace(nameof(lng)).Replace("DOT", ".");
            var attributes = new Dictionary<string, string>
            {
                { "Latitude", parsedLat },
                { "Longitude", parsedLng }
            };

            await _deviceManager.AddDeviceAttributesAsync(name, attributes, token);

            return Ok();
        }
    }
}
