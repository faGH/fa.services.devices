using FrostAura.Services.Devices.Data.Models.Entities;
using HotChocolate.Types;

namespace FrostAura.Services.Devices.Data.GraphQl.Types
{
    /// <summary>
    /// GraphQL device attribute type.
    /// </summary>
    public class DeviceAttributeType : ObjectType<DeviceAttribute>
    {
        /// <summary>
        /// Custom object descriptions.
        /// </summary>
        /// <param name="descriptor">Object descriptor.</param>
        protected override void Configure(IObjectTypeDescriptor<DeviceAttribute> descriptor)
        {
            descriptor
                .Field(d => d.Id)
                .Description("The device attribute's unique, auto-generated identifier.")
                .Type<IdType>();
            descriptor
                .Field(d => d.Value)
                .Description("The attribute's value as provided by the source.")
                .Type<StringType>();
            descriptor
                .Field(d => d.Device)
                .Description("The device that this value is for.")
                .Type<DeviceType>();
            descriptor
                .Field(d => d.Device)
                .Description("The attribute that this value is for.")
                .Type<AttributeType>();
            descriptor.Authorize();
        }
    }
}
