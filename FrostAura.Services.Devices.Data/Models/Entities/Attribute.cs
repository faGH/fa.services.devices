using FrostAura.Libraries.Data.Models.EntityFramework;
using FrostAura.Services.Devices.Data.GraphQl.Types;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrostAura.Services.Devices.Data.Models.Entities
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
        [UsePaging(SchemaType = typeof(DeviceAttributeType))]
        [UseFiltering]
        [UseSorting]
        public virtual ICollection<DeviceAttribute> DeviceAttributes { get; set; } = new List<DeviceAttribute>();
    }
}
