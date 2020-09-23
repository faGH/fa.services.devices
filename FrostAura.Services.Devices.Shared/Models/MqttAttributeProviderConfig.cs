using System.Collections.Generic;

namespace FrostAura.Services.Devices.Shared.Models
{
    /// <summary>
    /// Configuration model for an attribute provider.
    /// </summary>
    public class MqttAttributeProviderConfig
    {
        /// <summary>
        /// Server to connect to.
        /// </summary>
        public string Server { get; set; }
        /// <summary>
        /// Port to connect over.
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// Username credential.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Password credential.
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Topic to listen on.
        /// </summary>
        public string Topic { get; set; }
        /// <summary>
        /// Property mappings to transform incoming topic messages to device attributes.
        /// </summary>
        public List<AttributeMappingConfig> Mappings { get; set; }
    }
}
