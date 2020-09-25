using FrostAura.Services.Devices.Shared.Models.Entities;
using HotChocolate.Types;

namespace FrostAura.Services.Devices.Data.GraphQl.Types
{
    public class DeviceType : ObjectType<Device>
    {
        protected override void Configure(IObjectTypeDescriptor<Device> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Field(d => d.Id)
                .Type<IdType>();
        }
    }
}
