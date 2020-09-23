using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FrostAura.Services.Devices.Api.Controllers
{
    /// <summary>
    /// Controller for managing devices.
    /// </summary>
    public class DevicesController : BaseController
    {
        /// <summary>
        /// Overloaded constructor for injecting dependencies.
        /// </summary>
        /// <param name="logger">Controller logger.</param>
        public DevicesController(ILogger<BaseController> logger)
            : base(logger)
        { }

        [HttpGet]
        public async Task<IActionResult> GetAllDevices()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{deviceId}")]
        public async Task<IActionResult> GetSpecificDevice(string deviceId)
        {
            throw new NotImplementedException();
        }

        [HttpPost("{deviceId}")]
        [HttpPatch("{deviceId}")]
        public async Task<IActionResult> UpsertDevice()
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{deviceId}")]
        public async Task<IActionResult> DeleteDevice()
        {
            throw new NotImplementedException();
        }
    }
}
