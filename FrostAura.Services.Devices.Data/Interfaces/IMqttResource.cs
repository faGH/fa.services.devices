using System;
using System.Threading;
using System.Threading.Tasks;

namespace FrostAura.Services.Devices.Data.Interfaces
{
    /// <summary>
    /// MQTT manager.
    /// </summary>
    public interface IMqttResource
    {
        /// <summary>
        /// Event emitter for when a new message is received.
        /// <param type="string">Stringified payload.</param>
        /// </summary>
        event Action<string> OnMessage;
        /// <summary>
        /// Initialize the manager.
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        Task InitializeAsync(CancellationToken token);
    }
}
