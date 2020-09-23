using FrostAura.Libraries.Data.Models.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrostAura.Services.Devices.Shared.Models.Entities
{
    /// <summary>
    /// Attribute entity.
    /// </summary>
    [Table("Attributes")]
    public class Attribute : BaseEntity
    {
        /// <summary>
        /// Attribute name.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "A valid name is required.")]
        public string Name { get; set; }
        /// <summary>
        /// Device attributes.
        /// </summary>
        public virtual ICollection<DeviceAttribute> Devices { get; set; } = new List<DeviceAttribute>();
    }
}
