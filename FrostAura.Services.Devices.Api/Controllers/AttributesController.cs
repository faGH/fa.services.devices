using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FrostAura.Services.Devices.Api.Controllers
{
    /// <summary>
    /// Controller for managing device attributes.
    /// </summary>
    public class AttributesController : BaseController
    {
        /// <summary>
        /// Overloaded constructor for injecting dependencies.
        /// </summary>
        /// <param name="logger">Controller logger.</param>
        public AttributesController(ILogger<BaseController> logger)
            : base(logger)
        { }

        [HttpGet]
        public async Task<IActionResult> GetExistingAttributeNames()
        {
            throw new NotImplementedException();
        }

        [HttpGet("all/device/{deviceId}")]
        public async Task<IActionResult> GetAllAttributesForDevice(string deviceId)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{attrId}/device/{deviceId}")]
        public async Task<IActionResult> GetSpecificAttributeForDevice(string deviceId, string attrId)
        {
            throw new NotImplementedException();
        }

        [HttpPost("{attrId}/device/{deviceId}")]
        [HttpPatch("{attrId}/device/{deviceId}")]
        public async Task<IActionResult> UpsertSpecificAttributeForDevice(string deviceId, string attrId)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("device/{deviceId}")]
        public async Task<IActionResult> RemoveAllDeviceAttribute(string deviceId)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{attrId}/device/{deviceId}")]
        public async Task<IActionResult> RemoveSpecificDeviceAttribute(string deviceId, string attrId)
        {
            throw new NotImplementedException();
        }
    }
}
