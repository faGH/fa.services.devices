using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FrostAura.Services.Devices.Api.Controllers
{
    /// <summary>
    /// Base for all FrostAura controllers.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public abstract class BaseController : ControllerBase
    {
        /// <summary>
        /// Controller logger.
        /// </summary>
        protected readonly ILogger<BaseController> _logger;

        /// <summary>
        /// Overloaded constructor for injecting dependencies.
        /// </summary>
        /// <param name="logger">Controller logger.</param>
        protected BaseController(ILogger<BaseController> logger)
        {
            _logger = logger;
        }
    }
}
