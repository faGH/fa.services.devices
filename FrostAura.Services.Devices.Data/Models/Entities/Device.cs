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
    /// Device entity.
    /// </summary>
    [Table("Devices")]
    public class Device : BaseEntity
    {
        /// <summary>
        /// Unique device name from origin platform.
        /// NOTE: This is a unique index field. See OnModelCreating in DevicesDbContext.cs for more.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "A valid name is required.")]
        public string Name { get; set; }
        /// <summary>
        /// Device attributes.
        /// </summary>
        [UsePaging(SchemaType = typeof(DeviceAttributeType))]
        [UseFiltering]
        [UseSorting]
        public virtual ICollection<DeviceAttribute> Attributes { get; set; } = new List<DeviceAttribute>();
    }
}
