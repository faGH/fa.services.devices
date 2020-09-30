using FrostAura.Services.Devices.Data.Models.Entities;
using HotChocolate.Types;

namespace FrostAura.Services.Devices.Data.GraphQl.Types
{
    /// <summary>
    /// GraphQL attribute type.
    /// </summary>
    public class AttributeType : ObjectType<Attribute>
    {
        /// <summary>
        /// Custom object descriptions.
        /// </summary>
        /// <param name="descriptor">Object descriptor.</param>
        protected override void Configure(IObjectTypeDescriptor<Attribute> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .Field(a => a.Id)
                .Description("The attribute's unique, auto-generated identifier.")
                .Type<IdType>();
            descriptor
                .Field(d => d.Name)
                .Description("The attribute's name as provided by the source of the values.")
                .Type<StringType>();
            descriptor
                .Field(d => d.DeviceAttributes)
                .Description("A collection of all device attribute values.")
                .Type<ListType<DeviceAttributeType>>();
        }
    }
}
