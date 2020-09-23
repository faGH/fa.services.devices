namespace FrostAura.Services.Devices.Shared.Models
{
    /// <summary>
    /// Configuration model for attribute mappings.
    /// </summary>
    public class AttributeMappingConfig
    {
        /// <summary>
        /// Source field name to transform.
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// Destination attribute name to use.
        /// </summary>
        public string Destination { get; set; }
        /// <summary>
        /// Whether a given source field should act as the device identifier.
        /// </summary>
        public bool IsDeviceIdentifier { get; set; }
    }
}
