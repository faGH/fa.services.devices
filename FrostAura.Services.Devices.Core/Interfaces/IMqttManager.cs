using System.Threading;
using System.Threading.Tasks;

namespace FrostAura.Services.Devices.Core.Interfaces
{
    /// <summary>
    /// MQTT manager.
    /// </summary>
    public interface IMqttManager
    {
        /// <summary>
        /// Initialize the manager.
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        Task InitializeAsync(CancellationToken token);
    }
}
