using FrostAura.Libraries.Data.Models.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrostAura.Services.Devices.Data.Models.Entities
{
    /// <summary>
    /// Device attribute entity.
    /// </summary>
    [Table("DeviceAttributes")]
    public class DeviceAttribute : BaseEntity
    {
        /// <summary>
        /// Attribute value for the given device.
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Unique device identifier that this value setting is for.
        /// </summary>
        [Required]
        public int DeviceId { get; set; }
        /// <summary>
        /// Unique attribute identifier that this value setting is for.
        /// </summary>
        [Required]
        public int AttributeId { get; set; }
        /// <summary>
        /// Device instance.
        /// </summary>
        [ForeignKey("DeviceId")]
        public virtual Device Device { get; set; }
        /// <summary>
        /// Attribute instance.
        /// </summary>
        [ForeignKey("AttributeId")]
        public virtual Attribute Attribute { get; set; }
    }
}
