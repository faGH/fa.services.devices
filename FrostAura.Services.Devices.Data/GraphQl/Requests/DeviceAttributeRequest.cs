using FrostAura.Libraries.Core.Attributes.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FrostAura.Services.Devices.Data.GraphQl.Requests
{
    /// <summary>
    /// GraphQL device attribute DTO.
    /// </summary>
    public class DeviceAttributeRequest
    {
        /// <summary>
        /// Device's unique auto-generated id.
        /// NOTE: This should only be provided when an update has to take place.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "A valid device name is required..")]
        public string DeviceName { get; set; }
        /// <summary>
        /// Attributes and their values.
        /// Key/value pair where:
        ///     key => attribute name
        ///     value => attribute value
        /// </summary>
        //TODO: Fix validation issue. [CollectionValidation(AllowEmptyCollection = false, ErrorMessage = "One or more attributes are required.", ValidateCollectionEntries = false)]
        public List<KeyValuePair<string, string>> Attributes { get; set; }
    }
}
