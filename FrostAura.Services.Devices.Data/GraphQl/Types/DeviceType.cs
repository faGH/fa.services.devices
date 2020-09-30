using FrostAura.Services.Devices.Data.Models.Entities;
using HotChocolate.Types;

namespace FrostAura.Services.Devices.Data.GraphQl.Types
{
    /// <summary>
    /// GraphQL device type.
    /// </summary>
    public class DeviceType : ObjectType<Device>
    {
        /// <summary>
        /// Custom object descriptions.
        /// </summary>
        /// <param name="descriptor">Object descriptor.</param>
        protected override void Configure(IObjectTypeDescriptor<Device> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .Field(d => d.Id)
                .Description("The device's unique, auto-generated identifier.")
                .Type<IdType>();
            descriptor
                .Field(d => d.Name)
                .Description("The device's unique identifier as provided by the source.")
                .Type<StringType>();
            descriptor
                .Field(d => d.Attributes)
                .Description("A collection of all device attribute values.")
                .Type<ListType<DeviceAttributeType>>();
        }
    }
}
