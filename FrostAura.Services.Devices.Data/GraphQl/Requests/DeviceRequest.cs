namespace FrostAura.Services.Devices.Data.GraphQl.Requests
{
    /// <summary>
    /// GraphQL device DTO.
    /// </summary>
    public class DeviceRequest
    {
        /// <summary>
        /// Device's unique auto-generated id.
        /// NOTE: This should only be provided when an update has to take place.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Device's unique name.
        /// </summary>
        public string Name { get; set; }
    }
}
